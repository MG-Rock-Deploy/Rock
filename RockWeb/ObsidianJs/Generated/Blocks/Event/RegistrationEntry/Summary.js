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
System.register(["vue", "../../../Controls/GatewayControl", "../../../Controls/RockForm", "../../../Controls/RockValidation", "../../../Elements/Alert", "../../../Elements/CheckBox", "../../../Elements/EmailBox", "../../../Elements/JavaScriptAnchor", "../../../Elements/RockButton", "../../../Elements/TextBox", "../../../Services/Number", "../RegistrationEntry", "./RegistrationEntryBlockViewModel"], function (exports_1, context_1) {
    "use strict";
    var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
        function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
        return new (P || (P = Promise))(function (resolve, reject) {
            function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
            function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
            function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
            step((generator = generator.apply(thisArg, _arguments || [])).next());
        });
    };
    var __generator = (this && this.__generator) || function (thisArg, body) {
        var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
        return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
        function verb(n) { return function (v) { return step([n, v]); }; }
        function step(op) {
            if (f) throw new TypeError("Generator is already executing.");
            while (_) try {
                if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
                if (y = 0, t) op = [op[0] & 2, t.value];
                switch (op[0]) {
                    case 0: case 1: t = op; break;
                    case 4: _.label++; return { value: op[1], done: false };
                    case 5: _.label++; y = op[1]; op = [0]; continue;
                    case 7: op = _.ops.pop(); _.trys.pop(); continue;
                    default:
                        if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                        if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                        if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                        if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                        if (t[2]) _.ops.pop();
                        _.trys.pop(); continue;
                }
                op = body.call(thisArg, _);
            } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
            if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
        }
    };
    var vue_1, GatewayControl_1, RockForm_1, RockValidation_1, Alert_1, CheckBox_1, EmailBox_1, JavaScriptAnchor_1, RockButton_1, TextBox_1, Number_1, RegistrationEntry_1, RegistrationEntryBlockViewModel_1;
    var __moduleName = context_1 && context_1.id;
    return {
        setters: [
            function (vue_1_1) {
                vue_1 = vue_1_1;
            },
            function (GatewayControl_1_1) {
                GatewayControl_1 = GatewayControl_1_1;
            },
            function (RockForm_1_1) {
                RockForm_1 = RockForm_1_1;
            },
            function (RockValidation_1_1) {
                RockValidation_1 = RockValidation_1_1;
            },
            function (Alert_1_1) {
                Alert_1 = Alert_1_1;
            },
            function (CheckBox_1_1) {
                CheckBox_1 = CheckBox_1_1;
            },
            function (EmailBox_1_1) {
                EmailBox_1 = EmailBox_1_1;
            },
            function (JavaScriptAnchor_1_1) {
                JavaScriptAnchor_1 = JavaScriptAnchor_1_1;
            },
            function (RockButton_1_1) {
                RockButton_1 = RockButton_1_1;
            },
            function (TextBox_1_1) {
                TextBox_1 = TextBox_1_1;
            },
            function (Number_1_1) {
                Number_1 = Number_1_1;
            },
            function (RegistrationEntry_1_1) {
                RegistrationEntry_1 = RegistrationEntry_1_1;
            },
            function (RegistrationEntryBlockViewModel_1_1) {
                RegistrationEntryBlockViewModel_1 = RegistrationEntryBlockViewModel_1_1;
            }
        ],
        execute: function () {
            exports_1("default", vue_1.defineComponent({
                name: 'Event.RegistrationEntry.Summary',
                components: {
                    RockButton: RockButton_1.default,
                    TextBox: TextBox_1.default,
                    CheckBox: CheckBox_1.default,
                    EmailBox: EmailBox_1.default,
                    RockForm: RockForm_1.default,
                    Alert: Alert_1.default,
                    GatewayControl: GatewayControl_1.default,
                    RockValidation: RockValidation_1.default,
                    JavaScriptAnchor: JavaScriptAnchor_1.default
                },
                setup: function () {
                    return {
                        invokeBlockAction: vue_1.inject('invokeBlockAction'),
                        registrationEntryState: vue_1.inject('registrationEntryState')
                    };
                },
                data: function () {
                    return {
                        /** Is there an AJAX call in-flight? */
                        loading: false,
                        /** The bound value to the discount code input */
                        discountCodeInput: '',
                        /** A warning message about the discount code that is a result of a failed AJAX call */
                        discountCodeWarningMessage: '',
                        /** A success message about the discount code that is a result of a successful AJAX call */
                        discountCodeSuccessMessage: '',
                        /** The dollar amount to be discounted because of the discount code entered. */
                        discountAmount: 0,
                        /** The percent of the total to be discounted because of the discount code entered. */
                        discountPercent: 0,
                        /** Should the gateway control submit to the gateway to create a token? */
                        doGatewayControlSubmit: false,
                        /** Gateway indicated error */
                        gatewayErrorMessage: '',
                        /** Gateway indicated validation issues */
                        gatewayValidationFields: {}
                    };
                },
                computed: {
                    /** The settings for the gateway (MyWell, etc) control */
                    gatewayControlModel: function () {
                        return this.viewModel.GatewayControl;
                    },
                    /** The person that is currently authenticated */
                    currentPerson: function () {
                        return this.$store.state.currentPerson;
                    },
                    /** The person entering the registration information. This object is part of the registration state. */
                    registrar: function () {
                        return this.registrationEntryState.Registrar;
                    },
                    /** The first registrant entered into the registration. */
                    firstRegistrant: function () {
                        return this.registrationEntryState.Registrants[0];
                    },
                    /** This is the data sent from the C# code behind when the block initialized. */
                    viewModel: function () {
                        return this.registrationEntryState.ViewModel;
                    },
                    /** Should the checkbox allowing the registrar to choose to update their email address be shown? */
                    doShowUpdateEmailOption: function () {
                        var _a;
                        return !this.viewModel.ForceEmailUpdate && !!((_a = this.currentPerson) === null || _a === void 0 ? void 0 : _a.Email);
                    },
                    /** Should the discount column in the fee table be shown? */
                    showDiscountCol: function () {
                        return this.discountPercent > 0 || this.discountAmount > 0;
                    },
                    /** The fee line items that will be displayed in the summary */
                    lineItems: function () {
                        var lineItems = [];
                        for (var _i = 0, _a = this.registrationEntryState.Registrants; _i < _a.length; _i++) {
                            var registrant = _a[_i];
                            var total = this.viewModel.Cost;
                            var discountedTotal = total;
                            var discountRemaining = 0;
                            if (this.discountAmount && total < this.discountAmount) {
                                discountRemaining = this.discountAmount - total;
                                discountedTotal = 0;
                            }
                            else if (this.discountAmount) {
                                discountedTotal = total - this.discountAmount;
                            }
                            else if (this.discountPercent) {
                                var discount = this.discountPercent >= 1 ?
                                    this.total :
                                    this.discountPercent <= 0 ?
                                        0 :
                                        (total * this.discountPercent);
                                discountedTotal = total - discount;
                            }
                            var info = RegistrationEntry_1.getRegistrantBasicInfo(registrant, this.viewModel.RegistrantForms);
                            var name_1 = registrant.IsOnWaitList ?
                                info.FirstName + " " + info.LastName + " (Waiting List)" :
                                info.FirstName + " " + info.LastName;
                            if (registrant.IsOnWaitList) {
                                total = 0;
                                discountedTotal = 0;
                            }
                            lineItems.push({
                                Key: registrant.Guid,
                                IsFee: false,
                                Description: name_1,
                                Amount: total,
                                AmountFormatted: Number_1.asFormattedString(total),
                                DiscountedAmount: discountedTotal,
                                DiscountedAmountFormatted: Number_1.asFormattedString(discountedTotal),
                                DiscountHelp: ''
                            });
                            // Don't show fees if on the waitlist
                            if (registrant.IsOnWaitList) {
                                continue;
                            }
                            for (var _b = 0, _c = this.viewModel.Fees; _b < _c.length; _b++) {
                                var fee = _c[_b];
                                for (var _d = 0, _e = fee.Items; _d < _e.length; _d++) {
                                    var feeItem = _e[_d];
                                    var qty = registrant.FeeQuantities[feeItem.Guid];
                                    if (!qty) {
                                        continue;
                                    }
                                    var itemTotal = qty * feeItem.Cost;
                                    var itemDiscountedTotal = itemTotal;
                                    if (fee.DiscountApplies) {
                                        if (itemTotal < discountRemaining) {
                                            discountRemaining -= itemTotal;
                                            itemDiscountedTotal = 0;
                                        }
                                        else if (discountRemaining) {
                                            itemDiscountedTotal -= discountRemaining;
                                            discountRemaining = 0;
                                        }
                                        else if (this.discountPercent) {
                                            var discount = this.discountPercent >= 1 ?
                                                itemTotal :
                                                this.discountPercent <= 0 ?
                                                    0 :
                                                    (itemTotal * this.discountPercent);
                                            itemDiscountedTotal = itemTotal - discount;
                                        }
                                    }
                                    lineItems.push({
                                        Key: registrant.Guid + "-" + feeItem.Guid,
                                        IsFee: true,
                                        Description: fee.Name + "-" + feeItem.Name + " (" + qty + " @ $" + Number_1.asFormattedString(feeItem.Cost) + ")",
                                        Amount: itemTotal,
                                        AmountFormatted: Number_1.asFormattedString(itemTotal),
                                        DiscountedAmount: itemDiscountedTotal,
                                        DiscountedAmountFormatted: Number_1.asFormattedString(itemDiscountedTotal),
                                        DiscountHelp: fee.DiscountApplies ? '' : 'This item is not eligible for the discount.'
                                    });
                                }
                            }
                        }
                        return lineItems;
                    },
                    /** The total amount of the registration before discounts */
                    total: function () {
                        var total = 0;
                        for (var _i = 0, _a = this.lineItems; _i < _a.length; _i++) {
                            var lineItem = _a[_i];
                            total += lineItem.Amount;
                        }
                        return total;
                    },
                    /** The total amount of the registration after discounts */
                    discountedTotal: function () {
                        var total = 0;
                        for (var _i = 0, _a = this.lineItems; _i < _a.length; _i++) {
                            var lineItem = _a[_i];
                            total += lineItem.DiscountedAmount;
                        }
                        return total;
                    },
                    /** The total before discounts as a formatted string */
                    totalFormatted: function () {
                        return "$" + Number_1.asFormattedString(this.total);
                    },
                    /** The total after discounts as a formatted string */
                    discountedTotalFormatted: function () {
                        return "$" + Number_1.asFormattedString(this.discountedTotal);
                    },
                },
                methods: {
                    /** User clicked the "previous" button */
                    onPrevious: function () {
                        this.$emit('previous');
                    },
                    /** User clicked the "finish" button */
                    onNext: function () {
                        this.loading = true;
                        this.gatewayErrorMessage = '';
                        this.gatewayValidationFields = {};
                        this.doGatewayControlSubmit = true;
                    },
                    /** Send a user input discount code to the server so the server can check and send back
                     *  the discount amount. */
                    tryDiscountCode: function () {
                        return __awaiter(this, void 0, void 0, function () {
                            var result, discountText;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        this.loading = true;
                                        _a.label = 1;
                                    case 1:
                                        _a.trys.push([1, , 3, 4]);
                                        return [4 /*yield*/, this.invokeBlockAction('CheckDiscountCode', {
                                                code: this.discountCodeInput
                                            })];
                                    case 2:
                                        result = _a.sent();
                                        if (result.isError || !result.data) {
                                            this.discountCodeWarningMessage = "'" + this.discountCodeInput + "' is not a valid Discount Code.";
                                        }
                                        else {
                                            this.discountCodeWarningMessage = '';
                                            this.discountAmount = result.data.DiscountAmount;
                                            this.discountPercent = result.data.DiscountPercentage;
                                            discountText = result.data.DiscountPercentage ?
                                                Number_1.asFormattedString(this.discountPercent * 100, 0) + "%" :
                                                "$" + Number_1.asFormattedString(this.discountAmount, 2);
                                            this.discountCodeSuccessMessage = "Your " + discountText + " discount code for all registrants was successfully applied.";
                                            this.registrationEntryState.DiscountCode = result.data.DiscountCode;
                                        }
                                        return [3 /*break*/, 4];
                                    case 3:
                                        this.loading = false;
                                        return [7 /*endfinally*/];
                                    case 4: return [2 /*return*/];
                                }
                            });
                        });
                    },
                    /** Prefill in the registrar form fields based on the admin's settings */
                    prefillRegistrar: function () {
                        // If the information is aleady recorded, do not change it
                        if (this.registrar.NickName || this.registrar.LastName || this.registrar.Email) {
                            return;
                        }
                        // If the option is to prompt or use the current person, prefill the current person if available
                        if (this.currentPerson &&
                            (this.viewModel.RegistrarOption === RegistrationEntryBlockViewModel_1.RegistrarOption.UseLoggedInPerson || this.viewModel.RegistrarOption === RegistrationEntryBlockViewModel_1.RegistrarOption.PromptForRegistrar)) {
                            this.registrar.NickName = this.currentPerson.NickName || this.currentPerson.FirstName || '';
                            this.registrar.LastName = this.currentPerson.LastName || '';
                            this.registrar.Email = this.currentPerson.Email || '';
                            return;
                        }
                        if (this.viewModel.RegistrarOption === RegistrationEntryBlockViewModel_1.RegistrarOption.PromptForRegistrar) {
                            return;
                        }
                        // If prefill or first-registrant, then the first registrants info is used (as least as a starting point)
                        if (this.viewModel.RegistrarOption === RegistrationEntryBlockViewModel_1.RegistrarOption.PrefillFirstRegistrant || this.viewModel.RegistrarOption === RegistrationEntryBlockViewModel_1.RegistrarOption.UseFirstRegistrant) {
                            var firstRegistrantInfo = RegistrationEntry_1.getRegistrantBasicInfo(this.firstRegistrant, this.viewModel.RegistrantForms);
                            this.registrar.NickName = firstRegistrantInfo.FirstName;
                            this.registrar.LastName = firstRegistrantInfo.LastName;
                            this.registrar.Email = firstRegistrantInfo.Email;
                            return;
                        }
                    },
                    /**
                     * The gateway indicated success and returned a token
                     * @param token
                     */
                    onGatewayControlSuccess: function (token) {
                        this.loading = false;
                        this.registrationEntryState.GatewayToken = token;
                        // TODO
                        // submit the payload to the server
                        this.$emit('next');
                    },
                    /**
                     * The gateway indicated an error
                     * @param message
                     */
                    onGatewayControlError: function (message) {
                        this.doGatewayControlSubmit = false;
                        this.loading = false;
                        this.gatewayErrorMessage = message;
                    },
                    /**
                     * The gateway wants the user to fix some fields
                     * @param invalidFields
                     */
                    onGatewayControlValidation: function (invalidFields) {
                        this.doGatewayControlSubmit = false;
                        this.loading = false;
                        this.gatewayValidationFields = invalidFields;
                    },
                },
                watch: {
                    currentPerson: {
                        immediate: true,
                        handler: function () {
                            this.prefillRegistrar();
                        }
                    }
                },
                template: "\n<div class=\"registrationentry-summary\">\n    <RockForm @submit=\"onNext\">\n        <div class=\"well\">\n            <h4>This Registration Was Completed By</h4>\n            <div class=\"row\">\n                <div class=\"col-md-6\">\n                    <TextBox label=\"First Name\" rules=\"required\" v-model=\"registrar.NickName\" />\n                </div>\n                <div class=\"col-md-6\">\n                    <TextBox label=\"Last Name\" rules=\"required\" v-model=\"registrar.LastName\" />\n                </div>\n            </div>\n            <div class=\"row\">\n                <div class=\"col-md-6\">\n                    <EmailBox label=\"Send Confirmation Emails To\" rules=\"required\" v-model=\"registrar.Email\" />\n                    <CheckBox v-if=\"doShowUpdateEmailOption\" label=\"Should Your Account Be Updated To Use This Email Address?\" v-model=\"registrar.UpdateEmail\" />\n                </div>\n            </div>\n        </div>\n\n        <div>\n            <h4>Payment Summary</h4>\n            <Alert v-if=\"discountCodeWarningMessage\" alertType=\"warning\">{{discountCodeWarningMessage}}</Alert>\n            <Alert v-if=\"discountCodeSuccessMessage\" alertType=\"success\">{{discountCodeSuccessMessage}}</Alert>\n            <div class=\"clearfix\">\n                <div class=\"form-group pull-right\">\n                    <label class=\"control-label\">Discount Code</label>\n                    <div class=\"input-group\">\n                        <input type=\"text\" :disabled=\"loading || !!discountCodeSuccessMessage\" class=\"form-control input-width-md input-sm\" v-model=\"discountCodeInput\" />\n                        <RockButton v-if=\"!discountCodeSuccessMessage\" btnSize=\"sm\" :isLoading=\"loading\" class=\"margin-l-sm\" @click=\"tryDiscountCode\">\n                            Apply\n                        </RockButton>\n                    </div>\n                </div>\n            </div>\n            <div class=\"fee-table\">\n                <div class=\"row hidden-xs fee-header\">\n                    <div class=\"col-sm-6\">\n                        <strong>Description</strong>\n                    </div>\n                    <div v-if=\"showDiscountCol\" class=\"col-sm-3 fee-value\">\n                        <strong>Discounted Amount</strong>\n                    </div>\n                    <div class=\"col-sm-3 fee-value\">\n                        <strong>Amount</strong>\n                    </div>\n                </div>\n                <div v-for=\"lineItem in lineItems\" :key=\"lineItem.Key\" class=\"row\" :class=\"lineItem.IsFee ? 'fee-row-fee' : 'fee-row-cost'\">\n                    <div class=\"col-sm-6 fee-caption\">\n                        {{lineItem.Description}}\n                    </div>\n                    <div v-if=\"showDiscountCol\" class=\"col-sm-3 fee-value\">\n                        <JavaScriptAnchor v-if=\"lineItem.DiscountHelp\" class=\"help\" :title=\"lineItem.DiscountHelp\">\n                            <i class=\"fa fa-info-circle\"></i>\n                        </JavaScriptAnchor>\n                        <span class=\"visible-xs-inline\">Discounted Amount:</span>\n                        $ {{lineItem.DiscountedAmountFormatted}}\n                    </div>\n                    <div class=\"col-sm-3 fee-value\">\n                        <span class=\"visible-xs-inline\">Amount:</span>\n                        $ {{lineItem.AmountFormatted}}\n                    </div>\n                </div>\n                <div class=\"row fee-row-total\">\n                    <div class=\"col-sm-6 fee-caption\">\n                        Total\n                    </div>\n                    <div v-if=\"showDiscountCol\" class=\"col-sm-3 fee-value\">\n                        <span class=\"visible-xs-inline\">Discounted Amount:</span>\n                        {{discountedTotalFormatted}}\n                    </div>\n                    <div class=\"col-sm-3 fee-value\">\n                        <span class=\"visible-xs-inline\">Amount:</span>\n                        {{totalFormatted}}\n                    </div>\n                </div>\n            </div>\n\n            <div class=\"row fee-totals\">\n                <div class=\"col-sm-offset-8 col-sm-4 fee-totals-options\">\n                    <div class=\"form-group static-control\">\n                        <label class=\"control-label\">Total Cost</label>\n                        <div class=\"control-wrapper\">\n                            <div class=\"form-control-static\">\n                                {{discountedTotalFormatted}}\n                            </div>\n                        </div>\n                    </div>\n                    <div class=\"form-group static-control\">\n                        <label class=\"control-label\">Amount Due</label>\n                        <div class=\"control-wrapper\">\n                            <div class=\"form-control-static\">\n                                {{discountedTotalFormatted}}\n                            </div>\n                        </div>\n                    </div>\n                </div>\n            </div>\n        </div>\n\n        <div class=\"well\">\n            <h4>Payment Method</h4>\n            <Alert v-if=\"gatewayErrorMessage\" alertType=\"danger\">{{gatewayErrorMessage}}</Alert>\n            <RockValidation :errors=\"gatewayValidationFields\" />\n            <div class=\"hosted-payment-control\">\n                <GatewayControl\n                    :gatewayControlModel=\"gatewayControlModel\"\n                    :submit=\"doGatewayControlSubmit\"\n                    @success=\"onGatewayControlSuccess\"\n                    @error=\"onGatewayControlError\"\n                    @validation=\"onGatewayControlValidation\" />\n            </div>\n        </div>\n\n        <div class=\"actions\">\n            <RockButton btnType=\"default\" @click=\"onPrevious\" :isLoading=\"loading\">\n                Previous\n            </RockButton>\n            <RockButton btnType=\"primary\" class=\"pull-right\" type=\"submit\" :isLoading=\"loading\">\n                Finish\n            </RockButton>\n        </div>\n    </RockForm>\n</div>"
            }));
        }
    };
});
//# sourceMappingURL=Summary.js.map