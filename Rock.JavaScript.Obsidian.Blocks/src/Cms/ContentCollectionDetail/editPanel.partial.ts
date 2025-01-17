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

import { computed, defineComponent, PropType, ref, watch } from "vue";
import AttributeValuesContainer from "@Obsidian/Controls/attributeValuesContainer";
import CheckBox from "@Obsidian/Controls/checkBox";
import CheckBoxList from "@Obsidian/Controls/checkBoxList";
import NumberBox from "@Obsidian/Controls/numberBox";
import TextBox from "@Obsidian/Controls/textBox";
import { watchPropertyChanges } from "@Obsidian/Utility/block";
import { propertyRef, updateRefValue } from "@Obsidian/Utility/component";
import { ContentCollectionBag } from "@Obsidian/ViewModels/Blocks/Cms/ContentCollectionDetail/contentCollectionBag";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";

/** The items that can be selected from the personalization checkbox list. */
const enablePersonalizationItems: ListItemBag[] = [
    {
        value: "segments",
        text: "Segments"
    },
    {
        value: "requestFilters",
        text: "Request Filters"
    }
];

export default defineComponent({
    name: "Cms.ContentCollectionDetail.EditPanel",

    components: {
        AttributeValuesContainer,
        CheckBox,
        CheckBoxList,
        NumberBox,
        TextBox
    },

    props: {
        /** The content collection that is being edited. */
        modelValue: {
            type: Object as PropType<ContentCollectionBag>,
            required: true
        }
    },

    emits: {
        "update:modelValue": (_value: ContentCollectionBag) => true,
        "propertyChanged": (_value: string) => true
    },

    setup(props, { emit }) {
        // #region Values

        const attributes = ref(props.modelValue.attributes ?? {});
        const attributeValues = ref(props.modelValue.attributeValues ?? {});
        const description = propertyRef(props.modelValue.description ?? "", "Description");
        const name = propertyRef(props.modelValue.name ?? "", "Name");
        const collectionKey = propertyRef(props.modelValue.collectionKey ?? "", "CollectionKey");
        const trendingEnabled = propertyRef(props.modelValue.trendingEnabled, "TrendingEnabled");
        const trendingWindowDay = propertyRef(props.modelValue.trendingWindowDay, "TrendingWindowDay");
        const trendingMaxItems = propertyRef(props.modelValue.trendingMaxItems, "TrendingMaxItems");
        const trendingGravity = propertyRef(props.modelValue.trendingGravity, "TrendingGravity");
        const enableSegments = propertyRef(props.modelValue.enableSegments, "EnableSegments");
        const enableRequestFilters = propertyRef(props.modelValue.enableRequestFilters, "EnableRequestFilters");

        // Set some sane defaults if this is a new collection.
        if (!props.modelValue.idKey) {
            trendingWindowDay.value = 30;
            trendingMaxItems.value = 10;
            trendingGravity.value = 1.1;
        }

        // The properties that are being edited. This should only contain
        // objects returned by propertyRef().
        const propRefs = [description, name, collectionKey, trendingEnabled, trendingWindowDay, trendingMaxItems, trendingGravity, enableSegments, enableRequestFilters];

        // #endregion

        // #region Computed Values

        // Handles the checkbox list selection for the personalization options.
        // Dynamically constructs the selected values from the individual values
        // and automatically sets those individual values on update.
        const enablePersonalization = computed({
            get(): string[] {
                const values: string[] = [];

                if (enableSegments.value) {
                    values.push("segments");
                }

                if (enableRequestFilters.value) {
                    values.push("requestFilters");
                }

                return values;
            },
            set(values: string[]) {
                enableSegments.value = values.includes("segments");
                enableRequestFilters.value = values.includes("requestFilters");
            }
        });

        // #endregion

        // #region Functions

        // #endregion

        // #region Event Handlers

        // #endregion

        // Watch for parental changes in our model value and update all our values.
        watch(() => props.modelValue, () => {
            updateRefValue(attributes, props.modelValue.attributes ?? {});
            updateRefValue(attributeValues, props.modelValue.attributeValues ?? {});
            updateRefValue(description, props.modelValue.description ?? "");
            updateRefValue(name, props.modelValue.name ?? "");
            updateRefValue(collectionKey, props.modelValue.collectionKey ?? "");
            updateRefValue(trendingEnabled, props.modelValue.trendingEnabled);
            updateRefValue(trendingWindowDay, props.modelValue.trendingWindowDay);
            updateRefValue(trendingMaxItems, props.modelValue.trendingMaxItems);
            updateRefValue(trendingGravity, props.modelValue.trendingGravity);
            updateRefValue(enableSegments, props.modelValue.enableSegments);
            updateRefValue(enableRequestFilters, props.modelValue.enableRequestFilters);
        });

        // Determines which values we want to track changes on (defined in the
        // array) and then emit a new object defined as newValue.
        watch([attributeValues, ...propRefs], () => {
            const newValue: ContentCollectionBag = {
                ...props.modelValue,
                attributeValues: attributeValues.value,
                description: description.value,
                name: name.value,
                collectionKey: collectionKey.value,
                trendingEnabled: trendingEnabled.value,
                trendingWindowDay: trendingWindowDay.value,
                trendingMaxItems: trendingMaxItems.value,
                trendingGravity: trendingGravity.value,
                enableSegments: enableSegments.value,
                enableRequestFilters: enableRequestFilters.value
            };

            emit("update:modelValue", newValue);
        });

        // Watch for any changes to props that represent properties and then
        // automatically emit which property changed.
        watchPropertyChanges(propRefs, emit);

        return {
            attributes,
            attributeValues,
            description,
            enablePersonalization,
            enablePersonalizationItems,
            name,
            collectionKey,
            trendingEnabled,
            trendingWindowDay,
            trendingMaxItems,
            trendingGravity
        };
    },

    template: `
<fieldset>
    <div class="row">
        <div class="col-md-6">
            <TextBox v-model="name"
                label="Name"
                rules="required" />
        </div>

    </div>

    <TextBox v-model="description"
        label="Description"
        textMode="multiline" />

    <div class="row">
        <div class="col-md-6">
            <TextBox v-model="collectionKey"
                label="Key"
                help="The unique key that will identify this collection."
                rules="required" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <CheckBox v-model="trendingEnabled"
                label="Enable Trending"
                help="Determines if trending metrics should be calculated on each run of the collection update job." />

            <div v-if="trendingEnabled">
                <div class="row">
                    <div class="col-md-6">
                        <NumberBox v-model="trendingWindowDay"
                            label="Trending Window"
                            help="Determines how many days of interactions to look at to determine trending items."
                            :decimalCount="0"
                            rules="required|gte:0">
                            <template #append>
                                <span class="input-group-addon">Days</span>
                            </template>
                        </NumberBox>
                    </div>

                    <div class="col-md-6">
                        <NumberBox v-model="trendingMaxItems"
                            label="Trending Item Count"
                            help="The number of items to mark as trending."
                            :decimalCount="0"
                            rules="required|gte:0" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <NumberBox v-model="trendingGravity"
                            label="Trending Gravity"
                            help="Gravity helps apply more weight to items that are newer. Selecting the correct gravity value can be a bit of trial and error, but we recommend that you start with the default value."
                            rules="required|gte:0" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <CheckBoxList v-model="enablePersonalization"
                label="Enable Personalization"
                help="Determines which personalization features are enabled."
                :items="enablePersonalizationItems" />
        </div>
    </div>

    <AttributeValuesContainer v-model="attributeValues" :attributes="attributes" isEditMode :numberOfColumns="2" />
</fieldset>
`
});
