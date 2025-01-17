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

import { ContentCollectionFilterControl } from "@Obsidian/Enums/Cms/contentCollectionFilterControl";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";

/**
 * Identifies a single search filter that should be displayed to the
 * individual for them to limit the results that will be returned.
 */
export type SearchFilterBag = {
    /** Gets or sets the label to identify the search filter. */
    label?: string | null;

    /** Gets or sets the control type to use when rendering the filter. */
    control: ContentCollectionFilterControl;

    /**
     * Gets or sets a value indicating whether this filter supports
     * multiple selection.
     */
    isMultipleSelection: boolean;

    /** Gets or sets the items to allow the individual to pick from. */
    items?: ListItemBag[] | null;

    /** Gets or sets the markup to display above the filter control. */
    headerMarkup?: string | null;
};
