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

import { Guid } from "@Obsidian/Types";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";

/** Identifies a single tag to the EntityTagList control. */
export type EntityTagListTagBag = {
    /** Gets or sets the identifier key of the tag. */
    idKey?: string | null;

    /** Gets or sets the entity type unique identifier. */
    entityTypeGuid?: Guid | null;

    /** Gets or sets the name of the tag. */
    name?: string | null;

    /** Gets or sets the icon CSS class to display with tag. */
    iconCssClass?: string | null;

    /** Gets or sets the color of the background of the tag. */
    backgroundColor?: string | null;

    /** Gets or sets the category the tag belongs to. */
    category?: ListItemBag | null;

    /** Gets or sets a value indicating whether this tag is personal. */
    isPersonal: boolean;
};
