﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RequestFilterDetails.ascx.cs" Inherits="RockWeb.Blocks.Cms.RequestFilterDetails" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">
            <asp:HiddenField ID="hfRequestFilterId" runat="server" />
            <div class="panel-heading">
                <h1 class="panel-title">
                    <i class="fa fa-user-tag"></i>
                    <asp:Literal ID="lPanelTitle" runat="server" Text="" />

                </h1>
                <div class="panel-labels">
                    <Rock:HighlightLabel ID="hlInactive" runat="server" LabelType="Danger" Text="Inactive" />
                </div>
            </div>

            <div class="panel-body">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" />
                <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />
                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                <%-- Segment Name --%>
                <div class="row">
                    <div class="col-md-6">
                        <Rock:HiddenFieldWithClass ID="hfExistingSegmentKeyNames" runat="server" CssClass="js-existing-key-names" />
                        <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.PersonalizationSegment, Rock" PropertyName="Name" onblur="populateSegmentKey()" />
                        <Rock:DataTextBox ID="tbSegmentKey" runat="server" SourceTypeName="Rock.Model.PersonalizationSegment, Rock" PropertyName="SegmentKey" Label="Site" Help="Site - Optional site to limit the request filter to."/>
                    </div>

                    <div class="col-md-6">
                        <Rock:RockCheckBox ID="cbIsActive" runat="server" Label="Active" />
                    </div>
                </div>

                <%-- Person Filters --%>
                <asp:Panel ID="pnlPersonFilters" runat="server" CssClass="panel panel-section" Visible="false">
                    <div class="panel-heading">
                        <h1 class="panel-title">Previous Activity</h1>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:DataViewItemPicker ID="dvpFilterDataView" runat="server" Label="Filter Data View" OnSelectItem="dvpFilterDataView_SelectItem" />
                                <Rock:NotificationBox ID="nbFilterDataViewWarning" runat="server" NotificationBoxType="Danger"
                                    Text="Segments only support data views that have been configured to persist. Please update the configuration of the selected dataview." />
                            </div>

                            <div class="col-md-6">
                                Adding a data view to the segment will exclude anonymous visitors.
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <%-- Session Filters --%>
                <asp:Panel ID="pnlSessionCountFilters" runat="server" CssClass="panel panel-section" Visible="false">
                    <div class="panel-heading">
                        <div class="panel-title">Session Filters</div>
                        <Rock:Toggle ID="tglSessionCountFiltersAllAny" runat="server" OnText="All" OffText="Any" ActiveButtonCssClass="btn-info" ButtonSizeCssClass="btn-xs" />
                    </div>
                    <div class="panel-panel-body">
                        <Rock:Grid ID="gSessionCountFilters" runat="server" DisplayType="Light" RowItemText="Session Filter">
                            <Columns>
                                <Rock:RockLiteralField ID="lSessionCountFilterDescription" HeaderText="Description" OnDataBound="lSessionCountFilterDescription_DataBound" />
                                <Rock:EditField OnClick="gSessionCountFilters_EditClick" />
                                <Rock:DeleteField OnClick="gSessionCountFilters_DeleteClick" />
                            </Columns>
                        </Rock:Grid>
                    </div>
                </asp:Panel>

                <%-- Page View Filters --%>
                <asp:Panel ID="pnlPageViewFilters" runat="server" CssClass="panel panel-section" Visible="false">
                    <div class="panel-heading">
                        <div class="panel-title">Page View Filters</div>
                        <Rock:Toggle ID="tglPageViewFiltersAllAny" runat="server" OnText="All" OffText="Any" ActiveButtonCssClass="btn-info" ButtonSizeCssClass="btn-xs" />
                    </div>
                    <div class="panel-panel-body">
                        <Rock:Grid ID="gPageViewFilters" runat="server" DisplayType="Light" RowItemText="Page View Filter">
                            <Columns>
                                <Rock:RockLiteralField ID="lPageViewFilterDescription" HeaderText="Description" OnDataBound="lPageViewFilterDescription_DataBound" />
                                <Rock:EditField OnClick="gPageViewFilters_EditClick" />
                                <Rock:DeleteField OnClick="gPageViewFilters_DeleteClick" />
                            </Columns>
                        </Rock:Grid>
                    </div>

                </asp:Panel>

                <%-- Interaction Filters --%>
                <asp:Panel ID="pnlInteractionFilters" runat="server" CssClass="panel panel-section" Visible="false">
                    <div class="panel-heading">
                        <div class="panel-title">Interaction Filters</div>
                    </div>
                    <div class="panel-body">
                        <Rock:Grid ID="gInteractionFilters" runat="server" DisplayType="Light" RowItemText="Interaction Filter">
                            <Columns>
                                <Rock:RockBoundField DataField="InteractionChannelName" HeaderText="Channel" />
                                <Rock:RockBoundField DataField="InteractionComponentName" HeaderText="Component" />
                                <Rock:RockBoundField DataField="Operation" HeaderText="Operation"  />
                                <Rock:RockBoundField DataField="ComparisonText" HeaderText="Quantity"  />
                                <Rock:RockBoundField DataField="DateRangeText" HeaderText="Date Range" />
                                <Rock:EditField OnClick="gInteractionFilters_EditClick" />
                                <Rock:DeleteField OnClick="gInteractionFilters_DeleteClick" />
                            </Columns>
                        </Rock:Grid>
                    </div>
                </asp:Panel>

                
                <%-- Previous Activity --%>
                <asp:Panel ID="pnlPreviousActivity" runat="server" CssClass="panel panel-section">
                    <div class="panel-heading">
                        <div class="panel-title">Previous Activity</div>
                    </div>
                    <div class="panel-body">
                        <Rock:RockCheckBoxList ID="cblPreviousActivity" runat="server" Label="Vistor Type" RepeatDirection="Horizontal" />
                    </div>
                </asp:Panel>

                <span class="segment-and">--[AND]--</span>

                <%-- Device Types --%>
                <asp:Panel ID="pnlDeviceType" runat="server" CssClass="panel panel-section">
                    <div class="panel-heading">
                        <div class="panel-title">Device Types</div>
                    </div>
                    <div class="panel-body">
                        <Rock:RockCheckBoxList ID="cblDeviceTypes" runat="server" Label="Device Type" RepeatDirection="Horizontal" />
                    </div>
                </asp:Panel>

                <span class="segment-and">--[AND]--</span>

                <%-- Query String Filter --%>
                <asp:Panel ID="pnlQueryStringFilter" runat="server" CssClass="panel panel-section">
                    <div class="panel-heading">
                        <div class="panel-title pull-left">Query String Filter</div>
                        <Rock:Toggle ID="tglQueryStringFiltersAllAny" runat="server" OnText="All" OffText="Any" ActiveButtonCssClass="btn-info" ButtonSizeCssClass="btn-xs" CssClass="panel-title pull-right" />
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-body">
                        <Rock:Grid ID="gQueryStringFilter" runat="server" DisplayType="Light" RowItemText="Query String Filter">
                            <Columns>
                                <Rock:RockBoundField DataField="Key" HeaderText="Key" />
                                <Rock:RockBoundField DataField="MatchType" HeaderText="Match Type"  />
                                <Rock:RockBoundField DataField="Value" HeaderText="Value"  />
                                <Rock:EditField OnClick="gInteractionFilters_EditClick" />
                                <Rock:DeleteField OnClick="gInteractionFilters_DeleteClick" />
                            </Columns>
                        </Rock:Grid>
                    </div>
                </asp:Panel>

                <span class="segment-and">--[AND]--</span>

                <%-- Cookie --%>
                <asp:Panel ID="pnlCookie" runat="server" CssClass="panel panel-section">
                    <div class="panel-heading">
                        <div class="panel-title pull-left">Cookie</div>
                        <Rock:Toggle ID="tglCookiesAllAny" runat="server" OnText="All" OffText="Any" ActiveButtonCssClass="btn-info" ButtonSizeCssClass="btn-xs" CssClass="panel-title pull-right" />
                        <div class="clearfix"></div>                        
                    </div>
                    <div class="panel-body">
                        <Rock:Grid ID="gCookie" runat="server" DisplayType="Light" RowItemText="Cookie">
                            <Columns>
                                <Rock:RockBoundField DataField="Key" HeaderText="Key" />
                                <Rock:RockBoundField DataField="MatchType" HeaderText="Match Type"  />
                                <Rock:RockBoundField DataField="Value" HeaderText="Value"  />
                                <Rock:EditField OnClick="gInteractionFilters_EditClick" />
                                <Rock:DeleteField OnClick="gInteractionFilters_DeleteClick" />
                            </Columns>
                        </Rock:Grid>
                    </div>
                </asp:Panel>

                <span class="segment-and">--[AND]--</span>

                <%-- Browser --%>
                <asp:Panel ID="pnlBrowser" runat="server" CssClass="panel panel-section">
                    <div class="panel-heading">
                        <div class="panel-title">Browser</div>
                    </div>
                    <div class="panel-body">
                        <Rock:Grid ID="gBrowser" runat="server" DisplayType="Light" RowItemText="Browser">
                            <Columns>
                                <Rock:RockBoundField DataField="BrowserFamily" HeaderText="Browser Family" />
                                <Rock:RockBoundField DataField="MatchType" HeaderText="Match Type"  />
                                <Rock:RockBoundField DataField="MajorVersion" HeaderText="Major Version"  />
                                <Rock:EditField OnClick="gInteractionFilters_EditClick" />
                                <Rock:DeleteField OnClick="gInteractionFilters_DeleteClick" />
                            </Columns>
                        </Rock:Grid>
                    </div>
                </asp:Panel>

                <span class="segment-and">--[AND]--</span>

                <%-- IP Addresses --%>
                <asp:Panel ID="pnlIpAddress" runat="server" CssClass="panel panel-section">
                    <div class="panel-heading">
                        <div class="panel-title">IP Addresses</div>
                    </div>
                    <div class="panel-body">
                        <Rock:Grid ID="gIPAddress" runat="server" DisplayType="Light" RowItemText="IP Address">
                            <Columns>
                                <Rock:RockBoundField DataField="BeginningAddress" HeaderText="Beginning Address" />
                                <Rock:RockBoundField DataField="EndingAddress" HeaderText="Ending Address" />
                                <Rock:RockBoundField DataField="MatchType" HeaderText="Match Type"  />
                                <Rock:EditField OnClick="gInteractionFilters_EditClick" />
                                <Rock:DeleteField OnClick="gInteractionFilters_DeleteClick" />
                            </Columns>
                        </Rock:Grid>
                    </div>
                </asp:Panel>

                <div class="actions">
                    <asp:LinkButton ID="btnSave" runat="server" AccessKey="s" ToolTip="Alt+s" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" AccessKey="c" ToolTip="Alt+c" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>
            </div>

            <%-- Modal for Query String Filter --%>
            <Rock:ModalDialog ID="mdQueryStringFilter" runat="server" OnSaveClick="mdQueryStringFilter_SaveClick" ValidationGroup="vgQueryStringFilter">
                <Content>
                    <div class="panel-body">
                        <asp:HiddenField ID="hfQueryStringFilter" runat="server" />

                        <asp:ValidationSummary ID="vsQueryFilterString" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="vgQueryFilterString" />

                        <div class="container">
                            <div class="row">
                                <span class="col-sm-2">Where the parameter </span>
                                <Rock:RockTextBox ID="tbQueryStringFilterParameter" runat="server" CssClass="col-sm-4"/>
                                <Rock:RockDropDownList ID="ddlQueryStringFilterMatchOptions" runat="server" CssClass="col-sm-2" />
                            </div>
                        </div>


                        <span>the value</span>
                        
                        <Rock:RockTextBox ID="tbQueryStringFilterValue" runat="server" />

                    </div>


                </Content>
            </Rock:ModalDialog>

            <%-- Modal for Cookie --%>
            <Rock:ModalDialog ID="mdCookie" runat="server" OnSaveClick="mdCookie_SaveClick" ValidationGroup="vgCookie">
                <Content>
                    <div class="panel-body">
                        <asp:HiddenField ID="hfCookie" runat="server" />

                        <asp:ValidationSummary ID="vsCookie" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="vgCookie" />

                        <div class="container">
                            <div class="row">
                                <span class="col-sm-2">Where the parameter </span>
                                <Rock:RockTextBox ID="tbCookieParameter" runat="server" CssClass="col-sm-4"/>
                                <Rock:RockDropDownList ID="ddlCookieMatchOptions" runat="server" CssClass="col-sm-2" />
                            </div>
                        </div>


                        <span>the value</span>
                        
                        <Rock:RockTextBox ID="tbCookieValue" runat="server" />

                    </div>


                </Content>
            </Rock:ModalDialog>

            <%-- Modal for Browser Filter --%>
            <Rock:ModalDialog ID="mdBrowser" runat="server" OnSaveClick="mdBrowser_SaveClick" ValidationGroup="vgBrowser">
                <Content>
                    <div class="panel-body">
                        <asp:HiddenField ID="hfBrowser" runat="server" />

                        <asp:ValidationSummary ID="vsBrowser" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="vgBrowser" />

                        <div class="container">
                            <div class="row">
                                <span class="col-sm-2">Where </span>
                                <Rock:RockDropDownList ID="ddlBrowserFamily" runat="server" CssClass="col-sm-2" />
                                <span class="col-sm-2"> version is </span>
                                <Rock:RockDropDownList ID="ddlBrowserMatchOptions" runat="server" CssClass="col-sm-2" />
                                <Rock:RockTextBox ID="tbBrowserVersion" runat="server" CssClass="col-sm-4"/>
                            </div>
                        </div>
                    </div>


                </Content>
            </Rock:ModalDialog>

            <%-- Modal for IP Address Filter --%>
            <Rock:ModalDialog ID="mdIPAddress" runat="server" OnSaveClick="mdIpAddress_SaveClick" ValidationGroup="vgIPAddress">
                <Content>
                    <div class="panel-body">
                        <asp:HiddenField ID="hfIPAddress" runat="server" />

                        <asp:ValidationSummary ID="vsIPAddress" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="vgIPAddress" />

                        <div class="container">
                            <div class="row">
                                <span class="col-sm-2">Where the client IP is </span>
                                <Rock:Toggle ID="tglIPAddressRange" runat="server" OnText="In Range" OffText="Not in Range"
                                    ActiveButtonCssClass="btn-info" ButtonSizeCssClass="btn-xs" />
                                <Rock:RockTextBox ID="tbIPAddressStartRange" runat="server" CssClass="col-sm-4"/>
                                <Rock:RockTextBox ID="tbIPAddressEndRange" runat="server" CssClass="col-sm-4"/>
                            </div>
                        </div>
                    </div>


                </Content>
            </Rock:ModalDialog>


            <%-- Modal for Session Count Filter --%>
            <Rock:ModalDialog ID="mdSessionCountFilterConfiguration" runat="server" OnSaveClick="mdSessionCountFilterConfiguration_SaveClick" ValidationGroup="vgSessionCountFilterConfiguration">
                <Content>
                    <div class="panel-body">
                        <asp:HiddenField ID="hfSessionCountFilterGuid" runat="server" />

                        <asp:ValidationSummary ID="vsSessionCountFilterConfiguration" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="vgSessionCountFilterConfiguration" />

                        <div class="field-criteria">
                            <Rock:RockDropDownList ID="ddlSessionCountFilterComparisonType" CssClass="js-filter-compare" runat="server" ValidationGroup="vgSessionCountFilterConfiguration" />
                            <Rock:NumberBox ID="nbSessionCountFilterCompareValue" runat="server" Required="true" CssClass="js-filter-control" ValidationGroup="vgSessionCountFilterConfiguration" />
                        </div>

                        <span>sessions on the</span>
                        <Rock:RockListBox ID="lstSessionCountFilterWebSites" runat="server" Required="true" ValidationGroup="vgSessionCountFilterConfiguration" RequiredErrorMessage="Website is required." />
                        <span>website(s)</span>

                        <br />

                        <span>In the following date range</span>
                        <Rock:SlidingDateRangePicker ID="drpSessionCountFilterSlidingDateRange" runat="server" Label="" PreviewLocation="Right" EnabledSlidingDateRangeTypes="Previous, Last, Current, DateRange" ValidationGroup="vgSessionCountFilterConfiguration" />
                    </div>


                </Content>
            </Rock:ModalDialog>

            <%-- Modal for Page Views Filter --%>
            <Rock:ModalDialog ID="mdPageViewFilterConfiguration" runat="server" OnSaveClick="mdPageViewFilterConfiguration_SaveClick" ValidationGroup="vgPageViewFilterConfiguration">
                <Content>
                    <div class="panel-body">
                        <asp:HiddenField ID="hfPageViewFilterGuid" runat="server" />

                        <asp:ValidationSummary ID="vsPageViewFilterConfiguration" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="vgPageViewFilterConfiguration" />

                        <div class="field-criteria">
                            <Rock:RockDropDownList ID="ddlPageViewFilterComparisonType" CssClass="js-filter-compare" runat="server" ValidationGroup="vgPageViewFilterConfiguration" />
                            <Rock:NumberBox ID="nbPageViewFilterCompareValue" runat="server" Required="true" CssClass="js-filter-control" ValidationGroup="vgPageViewFilterConfiguration" />
                        </div>

                        <span>page views on the</span>
                        <Rock:RockListBox ID="lstPageViewFilterWebSites" runat="server" Required="true" ValidationGroup="vgPageViewFilterConfiguration" RequiredErrorMessage="Web Site is required." />
                        <span>website(s)</span>

                        <br />

                        <span>In the following date range</span>
                        <Rock:SlidingDateRangePicker ID="drpPageViewFilterSlidingDateRange" runat="server" Label="" PreviewLocation="Right" EnabledSlidingDateRangeTypes="Previous, Last, Current, DateRange" ValidationGroup="vgPageViewFilterConfiguration" ToolTip="<div class='js-slidingdaterange-info'></>" />

                        <br />

                        <span>optionally limited to the following pages</span>

                        <Rock:PagePicker ID="ppPageViewFilterPages" runat="server" AllowMultiSelect="true" ValidationGroup="vgPageViewFilterConfiguration"  Label="Page Picker Instead?"/>
                    </div>


                </Content>
            </Rock:ModalDialog>

            <%-- Modal for Interactions Filter --%>
            <Rock:ModalDialog ID="mdInteractionFilterConfiguration" runat="server" OnSaveClick="mdInteractionFilterConfiguration_SaveClick" ValidationGroup="vgInteractionFilterConfiguration">
                <Content>
                    <div class="panel-body">
                        <asp:HiddenField ID="hfInteractionFilterGuid" runat="server" />

                        <asp:ValidationSummary ID="vsInteractionFilterConfiguration" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="vgInteractionFilterConfiguration" />

                        <div class="field-criteria">
                            <Rock:RockDropDownList ID="ddlInteractionFilterComparisonType" CssClass="js-filter-compare" runat="server" ValidationGroup="vgInteractionFilterConfiguration" />
                            <Rock:NumberBox ID="nbInteractionFilterCompareValue" runat="server" Required="true" CssClass="js-filter-control" ValidationGroup="vgInteractionFilterConfiguration" />
                        </div>

                        <span>interactions in the channel/component</span>

                        <Rock:InteractionChannelPicker ID="pInteractionFilterInteractionChannel" runat="server" Label="Channel" ValidationGroup="vgInteractionFilterConfiguration" Required="true" AutoPostBack="true" OnSelectedIndexChanged="pInteractionFilterInteractionChannel_SelectedIndexChanged" />
                        <Rock:InteractionComponentPicker ID="pInteractionFilterInteractionComponent" runat="server" Label="Component" ValidationGroup="vgInteractionFilterConfiguration" Required="false" />

                        <Rock:RockTextBox ID="tbInteractionFilterOperation" runat="server" Label="Operation" />

                        <span>In the following date range</span>
                        <Rock:SlidingDateRangePicker ID="drpInteractionFilterSlidingDateRange" runat="server" Label="" PreviewLocation="Right" EnabledSlidingDateRangeTypes="Previous, Last, Current, DateRange" ValidationGroup="vgInteractionFilterConfiguration" ToolTip="<div class='js-slidingdaterange-info'></>" />

                        <br />

                    </div>


                </Content>
            </Rock:ModalDialog>

        </asp:Panel>

        <script>
            function populateSegmentKey() {
                // if the segment key hasn't been filled in yet, populate it with the segment name minus whitespace and special chars
                var $keyControl = $('#<%=tbSegmentKey.ClientID%>');
                var keyValue = $keyControl.val();

                var reservedKeyJson = $('#<%=hfExistingSegmentKeyNames.ClientID%>').val();
                var reservedKeyNames = eval('(' + reservedKeyJson + ')');

                if ($keyControl.length && (keyValue == '')) {

                    keyValue = $('#<%=tbName.ClientID%>').val().replace(/[^a-zA-Z0-9_.\-]/g, '');
                    var newKeyValue = keyValue;

                    var i = 1;
                    while ($.inArray(newKeyValue, reservedKeyNames) >= 0) {
                        newKeyValue = keyValue + i++;
                    }

                    $keyControl.val(newKeyValue);
                }
            }

        </script>

    </ContentTemplate>
</asp:UpdatePanel>
