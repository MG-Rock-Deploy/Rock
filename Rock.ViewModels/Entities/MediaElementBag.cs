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
    /// MediaElement View Model
    /// </summary>
    public partial class MediaElementBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets a description of the Element.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or set the duration in seconds of media element.
        /// </summary>
        /// <value>
        /// A integer representing the duration in seconds of media element.
        /// </value>
        public int? DurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the file data JSON content that will be stored in
        /// the database.
        /// </summary>
        /// <value>
        /// The file data.
        /// </value>
        public string FileDataJson { get; set; }

        /// <summary>
        /// Gets or sets the MediaFolderId of the Rock.Model.MediaFolder that this MediaElement belongs to. This property is required.
        /// </summary>
        /// <value>
        /// A System.Int32 representing the MediaFolderId of the Rock.Model.MediaFolder that this MediaElement belongs to.
        /// </value>
        public int MediaFolderId { get; set; }

        /// <summary>
        /// Gets or sets the custom provider metric data for this instance.
        /// </summary>
        /// <value>
        /// The custom provider metric data for this instance.
        /// </value>
        public string MetricData { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Element. This property is required.
        /// </summary>
        /// <value>
        /// A System.String representing the name of the Element.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the System.DateTime this instance was created on the provider.
        /// </summary>
        /// <value>
        /// The System.DateTime this instance was created on the provider.
        /// </value>
        public DateTime? SourceCreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the custom provider data for this instance.
        /// </summary>
        /// <value>
        /// The custom provider data for this instance.
        /// </value>
        public string SourceData { get; set; }

        /// <summary>
        /// Gets or sets the provider's unique identifier for this instance.
        /// </summary>
        /// <value>
        /// The provider's unique identifier for this instance.
        /// </value>
        public string SourceKey { get; set; }

        /// <summary>
        /// Gets or sets the System.DateTime this instance was modified on the provider.
        /// </summary>
        /// <value>
        /// The System.DateTime this instance was modified on the provider.
        /// </value>
        public DateTime? SourceModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail data JSON content that will stored
        /// in the database.
        /// </summary>
        /// <value>
        /// The thumbnail data.
        /// </value>
        public string ThumbnailDataJson { get; set; }

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