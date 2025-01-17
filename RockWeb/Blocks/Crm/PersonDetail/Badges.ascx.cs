// <copyright>
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
using System;
using System.ComponentModel;
using System.Web.UI;

using Rock;
using Rock.Attribute;
using Rock.Model;
using Rock.Web.UI;
using Rock.Web.Cache;

namespace RockWeb.Blocks.Crm.PersonDetail
{
    [DisplayName( "Badges" )]
    [Category( "CRM > Person Detail" )]
    [Description( "Handles displaying badges for an entity." )]

    [BadgesField(
        "Badges",
        Key = AttributeKey.Badges,
        Description = "The badges to display.",
        Order = 0 )]

    [Rock.SystemGuid.BlockTypeGuid( Rock.SystemGuid.BlockType.BADGES )]
    public partial class Badges : ContextEntityBlock
    {
        #region Attribute Keys
        private static class AttributeKey
        {
            public const string Badges = "Badges";
        }
        #endregion Attribute Keys

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            if ( !Page.IsPostBack && Entity != null && Entity.Id != 0 )
            {
                string badgeList = GetAttributeValue( AttributeKey.Badges );
                if ( !string.IsNullOrWhiteSpace( badgeList ) )
                {
                    foreach ( string badgeGuid in badgeList.SplitDelimitedValues() )
                    {
                        Guid guid = badgeGuid.AsGuid();
                        if ( guid != Guid.Empty )
                        {
                            var badge = BadgeCache.Get( guid );
                            if ( badge != null )
                            {
                                blBadges.BadgeTypes.Add( badge );
                            }
                        }
                    }
                }
            }
        }
    }
}