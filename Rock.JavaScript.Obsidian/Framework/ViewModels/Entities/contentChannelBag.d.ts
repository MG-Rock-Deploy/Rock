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

import { PublicAttributeBag } from "@Obsidian/ViewModels/Utility/publicAttributeBag";

/** ContentChannel View Model */
export type ContentChannelBag = {
    /** Gets or sets the channel URL. */
    channelUrl?: string | null;

    /** Gets or sets a value indicating whether child items are manually ordered or not */
    childItemsManuallyOrdered: boolean;

    /** Gets or sets the Rock.Model.ContentChannelType identifier. */
    contentChannelTypeId: number;

    /** Gets or sets the type of the control to render when editing content for items of this type. */
    contentControlType: number;

    /** Gets or sets the description. */
    description?: string | null;

    /** Gets or sets a value indicating whether [enable RSS]. */
    enableRss: boolean;

    /** Gets or sets the icon CSS class. */
    iconCssClass?: string | null;

    /** Gets or sets a value indicating whether this instance is index enabled. */
    isIndexEnabled: boolean;

    /** Gets or sets a value indicating whether this content is structured. */
    isStructuredContent: boolean;

    /** Gets or sets a value indicating whether this instance is tagging enabled. */
    isTaggingEnabled: boolean;

    /** Gets or sets a value indicating whether items are manually ordered or not */
    itemsManuallyOrdered: boolean;

    /** Gets or sets the item tag category identifier. */
    itemTagCategoryId?: number | null;

    /** Gets or sets the item URL. */
    itemUrl?: string | null;

    /** Gets or sets the name. */
    name?: string | null;

    /** Gets or sets a value indicating whether [requires approval]. */
    requiresApproval: boolean;

    /** Gets or sets the root image directory to use when the HTML control type is used */
    rootImageDirectory?: string | null;

    /** Gets or sets the Structure Content Tool Id. */
    structuredContentToolValueId?: number | null;

    /** Gets or sets the number of minutes a feed can stay cached before refreshing it from the source. */
    timeToLive?: number | null;

    /** Gets or sets the created date time. */
    createdDateTime?: string | null;

    /** Gets or sets the modified date time. */
    modifiedDateTime?: string | null;

    /** Gets or sets the created by person alias identifier. */
    createdByPersonAliasId?: number | null;

    /** Gets or sets the modified by person alias identifier. */
    modifiedByPersonAliasId?: number | null;

    /** Gets or sets the identifier key of this entity. */
    idKey?: string | null;

    /** Gets or sets the attributes. */
    attributes?: Record<string, PublicAttributeBag> | null;

    /** Gets or sets the attribute values. */
    attributeValues?: Record<string, string> | null;
};