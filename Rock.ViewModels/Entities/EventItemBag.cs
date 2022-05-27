//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
using System.Linq;

using Rock.ViewModels.Utility;

namespace Rock.ViewModels.Entities
{
    /// <summary>
    /// EventItem View Model
    /// </summary>
    public partial class EventItemBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the PersonId of the Rock.Model.Person who approved this event.
        /// </summary>
        /// <value>
        /// A System.Int32 representing the PersonId of the Rock.Model.Person who approved this event.
        /// </value>
        public int? ApprovedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the date this event was approved.
        /// </summary>
        /// <value>
        /// A System.DateTime representing the date that this event was approved.
        /// </value>
        public DateTime? ApprovedOnDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Description of the EventItem.
        /// </summary>
        /// <value>
        /// A System.String representing the description of the EventItem.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL for an external event.
        /// </summary>
        /// <value>
        /// A System.String representing the URL for an external event.
        /// </value>
        public string DetailsUrl { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the event has been approved.
        /// </summary>
        /// <value>
        /// A System.Boolean value that is true if this event has been approved; otherwise false.
        /// </value>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the Name of the EventItem. This property is required.
        /// </summary>
        /// <value>
        /// A System.String representing the Name of the EventItem.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Rock.Model.BinaryFile that contains the photo of the EventItem.
        /// </summary>
        /// <value>
        /// A System.Int32 representing the Id of the Rock.Model.BinaryFile containing the photo of the EventItem.
        /// </value>
        public int? PhotoId { get; set; }

        /// <summary>
        /// Gets or sets the Summary of the EventItem.
        /// </summary>
        /// <value>
        /// A System.String representing the summary of the EventItem.
        /// </value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the modified date time.
        /// </summary>
        /// <value>
        /// The modified date time.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created by person alias identifier.
        /// </summary>
        /// <value>
        /// The created by person alias identifier.
        /// </value>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the modified by person alias identifier.
        /// </summary>
        /// <value>
        /// The modified by person alias identifier.
        /// </value>
        public int? ModifiedByPersonAliasId { get; set; }

    }
}