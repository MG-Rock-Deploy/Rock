﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using Rock.Data;
using Rock.Lava;
using Rock.Security;
using Rock.Web.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Rock.Model
{
    /// <summary>
    /// Note Logic
    /// </summary>
    public partial class Note
    {
        #region Virtual Properties

        /// <summary>
        /// Gets the childs note that the current person is allowed to view
        /// </summary>
        /// <value>
        /// The viewable child notes.
        /// </value>
        [LavaVisible]
        [NotMapped]
        public virtual List<Note> ViewableChildNotes
        {
            get
            {
                // only get notes they have auth to VIEW ( note that VIEW has special rules based on approval status, etc. See Note.IsAuthorized for details )
                var currentPerson = System.Web.HttpContext.Current?.Items["CurrentPerson"] as Person;

                var viewableChildNotes = ChildNotes.ToList().Where( a => a.IsAuthorized( Rock.Security.Authorization.VIEW, currentPerson ) ).ToList();

                return viewableChildNotes;
            }
        }

        /// <summary>
        /// Gets the created by person photo URL.
        /// </summary>
        /// <value>
        /// The created by person photo URL.
        /// </value>
        [LavaVisible]
        public virtual string CreatedByPersonPhotoUrl
        {
            get
            {
                return Person.GetPersonPhotoUrl( this.CreatedByPersonAlias.Person );
            }
        }

        /// <summary>
        /// Gets the id to use in the note's anchor tag
        /// </summary>
        /// <value>
        /// The note anchor identifier.
        /// </value>
        [LavaVisible]
        public virtual string NoteAnchorId => $"NoteRef-{this.Guid.ToString( "N" )}";

        /// <summary>
        /// Gets the name of the person that last edited the note text. Use this instead of ModifiedByPersonName to determine the last person to edit the note text
        /// </summary>
        /// <value>
        /// The edited by person alias.
        /// </value>
        [LavaVisible]
        public virtual string EditedByPersonName
        {
            get
            {
                var editedByPerson = EditedByPersonAlias?.Person ?? CreatedByPersonAlias?.Person;
                return editedByPerson?.FullName;
            }
        }

        /// <summary>
        /// Gets the name of the entity (If it is a Note on a Person, it would be the person's name, etc)
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        [LavaVisible]
        public virtual string EntityName
        {
            get
            {
                using ( var rockContext = new RockContext() )
                {
                    var noteTypeEntityTypeId = NoteTypeCache.Get( this.NoteTypeId )?.EntityTypeId;
                    if ( noteTypeEntityTypeId.HasValue && this.EntityId.HasValue )
                    {
                        var entity = new EntityTypeService( rockContext ).GetEntity( this.NoteType.EntityTypeId, this.EntityId.Value );
                        return entity?.ToString();
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the approval URL.
        /// </summary>
        /// <value>
        /// The approval URL.
        /// </value>
        [LavaVisible]
        public virtual string ApprovalUrl
        {
            get
            {
                string approvalUrlTemplate = NoteTypeCache.Get( this.NoteTypeId )?.ApprovalUrlTemplate;
                if ( string.IsNullOrWhiteSpace( approvalUrlTemplate ) )
                {
                    approvalUrlTemplate = "{{ 'Global' | Attribute:'InternalApplicationRoot' }}{{ Note.NoteUrl }}#{{ Note.NoteAnchorId }}";
                }

                var mergeFields = new Dictionary<string, object> { { "Note", this } };

                string approvalUrl = approvalUrlTemplate.ResolveMergeFields( mergeFields );

                return approvalUrl;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the currently logged in person is watching this specific note
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is current person watching; otherwise, <c>false</c>.
        /// </value>
        [LavaVisible]
        public virtual bool IsCurrentPersonWatching
        {
            get
            {
                var currentPerson = System.Web.HttpContext.Current?.Items["CurrentPerson"] as Person;
                var currentPersonId = currentPerson?.Id;
                if ( currentPersonId.HasValue )
                {
                    using ( var rockContext = new RockContext() )
                    {
                        bool isWatching = new NoteWatchService( rockContext ).Queryable()
                                .Where( a => a.NoteId == this.Id 
                                    && a.WatcherPersonAlias.PersonId == currentPersonId.Value 
                                    && a.IsWatching == true ).Any();

                        return isWatching;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the count of that are descendants (replies) of this note.
        /// </summary>
        /// <value>
        /// The viewable descendents count.
        /// </value>
        [LavaVisible]
        public virtual int ViewableDescendentsCount
        {
            get
            {
                if ( !_viewableDescendentsCount.HasValue )
                {
                    var currentPerson = System.Web.HttpContext.Current?.Items["CurrentPerson"] as Person;

                    using ( var rockContext = new RockContext() )
                    {
                        var noteDescendents = new NoteService( rockContext ).GetAllDescendents( this.Id ).ToList();
                        var viewableDescendents = noteDescendents.ToList().Where( a => a.IsAuthorized( Rock.Security.Authorization.VIEW, currentPerson ) ).ToList();
                        _viewableDescendentsCount = viewableDescendents.Count();
                    }
                }

                return _viewableDescendentsCount.Value;
            }
        }

        private int? _viewableDescendentsCount = null;

        #endregion Virtual Properties

        #region Security Overrides

        /// <summary>
        /// Gets the parent security authority of this Note. Where security is inherited from.
        /// </summary>
        /// <value>
        /// The parent authority.
        /// </value>
        public override Security.ISecured ParentAuthority
        {
            get
            {
                var noteType = NoteTypeCache.Get( this.NoteTypeId );
                return noteType ?? base.ParentAuthority;
            }
        }

        /// <summary>
        /// Determines whether the specified action is authorized on this note.
        /// Special note on the VIEW action: a person can view a note if they have normal VIEW access, but also have any of the following is true of the note:
        ///  - Approved,
        ///  - The current person is the one who created the note,
        ///  - The current person is the one who last edited the note,
        ///  - No Approval is required,
        ///  - The current person is an approver
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public override bool IsAuthorized( string action, Person person )
        {
            if ( this.IsPrivateNote )
            {
                // If this is a private note, the creator has FULL access to it. Everybody else has NO access (including admins)
                if ( this.CreatedByPersonAlias?.PersonId == person?.Id )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if ( action.Equals( Rock.Security.Authorization.APPROVE, StringComparison.OrdinalIgnoreCase ) )
            {
                // If checking the APPROVE action, let people Approve private notes that they created (see above), otherwise just use the normal IsAuthorized
                return base.IsAuthorized( action, person );
            }
            else if ( action.Equals( Rock.Security.Authorization.VIEW, StringComparison.OrdinalIgnoreCase ) )
            {
                // View has special rules depending on the approval status and APPROVE verb

                // first check if have normal VIEW access on the base
                if ( !base.IsAuthorized( Authorization.VIEW, person ) )
                {
                    return false;
                }

                if ( this.ApprovalStatus == NoteApprovalStatus.Approved )
                {
                    return true;
                }
                else if ( this.CreatedByPersonAliasId == person?.PrimaryAliasId )
                {
                    return true;
                }
                else if ( this.EditedByPersonAliasId == person?.PrimaryAliasId )
                {
                    return true;
                }
                else if ( NoteTypeCache.Get( this.NoteTypeId )?.RequiresApprovals != true )
                {
                    /*
                    1/21/2021 - Shaun
                    If this Note does not have an assigned NoteType, it should be assumed that the NoteType does not
                    require approvals.  This is likely because a new instance of a Note entity was created to check
                    authorization for viewing Note entities in general, and in this case the first check (to
                    base.IsAuthorized) is sufficient to permit access.

                    Reason:  Notes should be available for DataViews.
                    */
                    return true;
                }
                else if ( this.IsAuthorized( Authorization.APPROVE, person ) )
                {
                    return true;
                }

                return false;
            }
            else if ( action.Equals( Rock.Security.Authorization.EDIT, StringComparison.OrdinalIgnoreCase ) )
            {
                // If this note was created by the logged person, they should be able to EDIT their own note,
                // otherwise EDIT (and DELETE) of other people's notes require ADMINISTRATE
                if ( CreatedByPersonAlias?.PersonId == person?.Id )
                {
                    return true;
                }
                else 
                {
                    return base.IsAuthorized( Rock.Security.Authorization.ADMINISTRATE, person );
                }
            }
            else
            {
                // If this note was created by the logged person, they should be able to do any action (except for APPROVE)
                if ( CreatedByPersonAlias?.PersonId == person?.Id )
                {
                    return true;
                }

                return base.IsAuthorized( action, person );
            }
        }

        /// <summary>
        /// Determines whether the specified action is private.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public override bool IsPrivate( string action, Person person )
        {
            if ( CreatedByPersonAlias != null && person != null &&
                CreatedByPersonAlias.PersonId == person.Id &&
                IsPrivateNote )
            {
                return true;
            }

            return base.IsPrivate( action, person );
        }

#endregion Security Overrides

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Text;
        }

        #endregion Public Methods
    }
}