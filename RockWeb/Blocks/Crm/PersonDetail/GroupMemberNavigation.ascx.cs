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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Crm.PersonDetail
{
    [DisplayName( "Family Navigation" )]
    [Category( "CRM > Person Detail" )]
    [Description( "Allows you to switch between the members of the family the person belongs to." )]

    [GroupTypeField( "Group Type",
        Description = "",
        Key = AttributeKey.GroupType,
        IsRequired = true,
        DefaultValue = Rock.SystemGuid.GroupType.GROUPTYPE_FAMILY )]

    [BooleanField( "Show Only Primary Group Members",
        Description = "",
        Key = AttributeKey.ShowOnlyPrimaryGroupMembers,
        IsRequired = true,
        DefaultBooleanValue = false )]

    [Rock.SystemGuid.BlockTypeGuid( "35D091FA-8311-42D1-83F7-3E67B9EE9675" )]
    public partial class GroupMemberNavigation : Rock.Web.UI.PersonBlock
    {
        #region Attribute Keys

        private static class AttributeKey
        {
            public const string GroupType = "GroupType";
            public const string ShowOnlyPrimaryGroupMembers = "ShowOnlyPrimaryGroupMembers";
        }

        #endregion Attribute Keys

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            this.BlockUpdated += Block_BlockUpdated;
        }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
            BuildDropDown();
        }

        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            BuildDropDown();
        }

        protected void BuildDropDown()
        {
            var sb = new StringBuilder(
            $@"<a href=""#"" id=""familyDropdownNav"" class=""profile-toggle"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
                    <img src=""{Person.GetPersonPhotoUrl( this.Person )}"" class=""avatar mr-2 flex-shrink-0"" alt="""" />
                    <span class=""d-none d-sm-inline text-nowrap font-weight-semibold"">{this.Person.FullName}<i class=""fa fa-chevron-down ml-2""></i></span>
                </a>

                <ul class=""dropdown-menu"" aria-labelledby=""familyDropdownNav"">");

            sb.AppendLine();

            var groupMembers = GetGroupMembers();
            foreach ( var groupMember in groupMembers )
            {
                sb.Append( CreateGroupMemberListItem( groupMember ) );
            }

            sb.AppendLine( "</ul>" );
            litGroupMemberNav.Text = sb.ToString();
        }

        private string CreateGroupMemberListItem( GroupMember groupMember )
        {
            var personLink = FormatPersonLink( groupMember.Person.Id.ToString() );
            var groupMemberListItem = $@"
                <li>
                    <a href=""{personLink}"">
                        <img src=""{Person.GetPersonPhotoUrl( groupMember.PersonId )}"" alt="""" class=""avatar"">
                        <span class=""name"">
                            {groupMember.Person.FullName}
                        </span>
                    </a>
                </li>";

            return groupMemberListItem;
        }

        private List<GroupMember> GetGroupMembers()
        {
            var groupTypeId = GroupTypeCache.GetId( GetAttributeValue( AttributeKey.GroupType ).AsGuid() );
            var showOnlyPrimaryGroup = GetAttributeValue( AttributeKey.ShowOnlyPrimaryGroupMembers ).AsBoolean();

            var rockContext = new RockContext();
            var groupMemberService = new GroupMemberService( rockContext );
            var orderedGroupMemberList = new List<GroupMember>();
            var groupMemberList = new List<GroupMember>();
            var groupIds = new List<int>();

            if ( showOnlyPrimaryGroup )
            {
                groupIds.Add( new GroupMemberService( rockContext )
                    .Queryable( true )
                    .Where( m => m.GroupTypeId == groupTypeId && m.PersonId == this.Person.Id )
                    .OrderBy( m => m.GroupOrder ?? int.MaxValue )
                    .ToList()
                    .Select( m => m.GroupId )
                    .FirstOrDefault() );
            }
            else
            {
                groupIds = groupMemberService
                    .Queryable( true )
                    .Where( m => m.GroupTypeId == groupTypeId && m.PersonId == this.Person.Id )
                    .OrderBy( m => m.GroupOrder ?? int.MaxValue )
                    .ToList()
                    .Select( m => m.GroupId )
                    .Distinct()
                    .ToList();
            }

            foreach ( var groupId in groupIds )
            {
                var members = new GroupMemberService( rockContext )
                    .Queryable( "GroupRole,Person", true )
                    .Where( m => m.GroupId == groupId && m.PersonId != this.Person.Id )
                    .OrderBy( m => m.GroupRole.Order )
                    .ThenBy( m => m.Id )
                    .ToList();

                // Add adult males
                orderedGroupMemberList.AddRange( members
                    .Where( m => m.GroupRole.Guid.Equals( new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) ) && m.Person.Gender == Gender.Male )
                    .OrderByDescending( m => m.Person.Age ) );

                // Add adult females
                orderedGroupMemberList.AddRange( members
                    .Where( m => m.GroupRole.Guid.Equals( new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) ) && m.Person.Gender != Gender.Male )
                    .OrderByDescending( m => m.Person.Age ) );

                // Add non-adults
                orderedGroupMemberList.AddRange( members
                    .Where( m => !m.GroupRole.Guid.Equals( new Guid( Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT ) ) )
                    .OrderByDescending( m => m.Person.Age ) );
            }

            return orderedGroupMemberList;
        }

        protected string FormatPersonLink( string personId )
        {
            var currentPersonId = Person.Id.ToString();

            if ( PageCache.PageContexts.ContainsKey( Person.TypeName ) )
            {
                currentPersonId = PageParameter( PageCache.PageContexts[Person.TypeName] );
            }

            // Look for a subpage route (anything after the "/Person/{id}" part of the URL)
            var subpageRoute = Request.UrlProxySafe().AbsolutePath.ReplaceCaseInsensitive( ResolveRockUrl( $"~/Person/{currentPersonId}" ), "" );

            // If the path is different, then append it onto the link
            if ( subpageRoute != Request.UrlProxySafe().AbsolutePath )
            {
                return ResolveRockUrl( string.Format( "~/Person/{0}{1}", personId, subpageRoute ) );
            }
            else
            {
                return ResolveRockUrl( string.Format( "~/Person/{0}", personId ) );
            }
        }
    }
}