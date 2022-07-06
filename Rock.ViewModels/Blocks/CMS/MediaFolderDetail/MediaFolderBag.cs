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

using System.Collections.Generic;

using Rock.ViewModels.Utility;

namespace Rock.ViewModels.Blocks.CMS.MediaFolderDetail
{
    public class MediaFolderBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the content channel identifier.
        /// </summary>
        /// <value>
        /// The content channel identifier.
        /// </value>
        public int? ContentChannelId { get; set; }
        /// <summary>
        /// Gets or sets the content channel.
        /// </summary>
        public ListItemBag ContentChannel { get; set; }

        /// <summary>
        /// Gets or sets the content channel attribute.
        /// </summary>
        public ListItemBag ContentChannelAttribute { get; set; }

        /// <summary>
        /// Gets or sets a description of the MediaFolder.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content channel sync is enabled.
        /// </summary>
        public bool IsContentChannelSyncEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this Media Folder is public.
        /// </summary>
        public bool? IsPublic { get; set; }

        /// <summary>
        /// Gets or sets the Media Account that this MediaFolder belongs to.
        /// </summary>
        public ListItemBag MediaAccount { get; set; }

        /// <summary>
        /// Gets or sets a collection containing the Elements that belong to this Folder.
        /// </summary>
        public List<ListItemBag> MediaElements { get; set; }

        /// <summary>
        /// Gets or sets the custom provider metric data for this instance.
        /// </summary>
        public string MetricData { get; set; }

        /// <summary>
        /// Gets or sets the Name of the MediaFolder. This property is required.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the custom provider data for this instance.
        /// </summary>
        public string SourceData { get; set; }

        /// <summary>
        /// Gets or sets the provider's unique identifier for this instance.
        /// </summary>
        public string SourceKey { get; set; }

        /// <summary>
        /// Gets or sets the type of the workflow that will be launched when a new Rock.Model.MediaElement is added.
        /// </summary>
        public ListItemBag WorkflowType { get; set; }

        /// <summary>
        /// Gets or sets the content channel status.
        /// </summary>
        /// <value>
        /// The content channel status.
        /// </value>
        public string ContentChannelStatus { get; set; }
    }
}
