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
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class AddAppleDevicesDefinedType : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.AddDefinedType( "Global", "Apple Device Models", "Apple device models.", SystemGuid.DefinedType.APPLE_DEVICE_MODELS );

            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "i386", "iPhone Simulator" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "x86_64", "iPhone Simulator" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "arm64", "iPhone Simulator" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone1,1", "iPhone" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone1,2", "iPhone 3G" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone2,1", "iPhone 3GS" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone3,1", "iPhone 4" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone3,2", "iPhone 4 GSM Rev A" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone3,3", "iPhone 4 CDMA" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone4,1", "iPhone 4S" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone5,1", "iPhone 5 (GSM)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone5,2", "iPhone 5 (GSM+CDMA)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone5,3", "iPhone 5C (GSM)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone5,4", "iPhone 5C (Global)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone6,1", "iPhone 5S (GSM)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone6,2", "iPhone 5S (Global)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone7,1", "iPhone 6 Plus" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone7,2", "iPhone 6" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone8,1", "iPhone 6s" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone8,2", "iPhone 6s Plus" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone8,4", "iPhone SE (GSM)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone9,1", "iPhone 7" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone9,2", "iPhone 7 Plus" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone9,3", "iPhone 7" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone9,4", "iPhone 7 Plus" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone10,1", "iPhone 8" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone10,2", "iPhone 8 Plus" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone10,3", "iPhone X Global" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone10,4", "iPhone 8" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone10,5", "iPhone 8 Plus" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone10,6", "iPhone X GSM" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone11,2", "iPhone XS" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone11,4", "iPhone XS Max" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone11,6", "iPhone XS Max Global" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone11,8", "iPhone XR" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone12,1", "iPhone 11" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone12,3", "iPhone 11 Pro" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone12,5", "iPhone 11 Pro Max" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone12,8", "iPhone SE 2nd Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone13,1", "iPhone 12 Mini" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone13,2", "iPhone 12" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone13,3", "iPhone 12 Pro" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone13,4", "iPhone 12 Pro Max" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone14,2", "iPhone 13 Pro" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone14,3", "iPhone Pro Max" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone14,4", "iPhone 13 Mini" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone14,5", "iPhone 13" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPhone14,6", "iPhone SE 3rd Gen" );

            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPod1,1", "1st Gen iPod" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPod2,1", "2nd Gen iPod" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPod3,1", "3rd Gen iPod" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPod4,1", "4th Gen iPod" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPod5,1", "5th Gen iPod" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPod6,1", "6th Gen iPod" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPod7,1", "7th Gen iPod" );

            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad1,1", "iPad" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad1,2", "iPad 3G" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad2,1", "2nd Gen iPad" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad2,2", "2nd Gen iPad GSM" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad2,3", "2nd Gen iPad CDMA" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad2,4", "2nd Gen iPad New Revision" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad3,1", "3rd Gen iPad" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad3,2", "3rd Gen iPad CDMA" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad3,3", "3rd Gen iPad GSM" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad2,5", "iPad mini" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad2,6", "iPad mini GSM+LTE" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad2,7", "iPad mini CDMA+LTE" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad3,4", "4th Gen iPad" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad3,5", "4th Gen iPad GSM+LTE" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad3,6", "4th Gen iPad CDMA+LTE" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,1", "iPad Air (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,2", "iPad Air (GSM+CDMA)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,3", "1st Gen iPad Air (China)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,4", "iPad mini Retina (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,5", "iPad mini Retina (GSM+CDMA)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,6", "iPad mini Retina (China)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,7", "iPad mini 3 (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,8", "iPad mini 3 (GSM+CDMA)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad4,9", "iPad Mini 3 (China)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad5,1", "iPad mini 4 (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad5,2", "4th Gen iPad mini (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad5,3", "iPad Air 2 (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad5,4", "iPad Air 2 (Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad6,3", "iPad Pro (9.7 inch, WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad6,4", "iPad Pro (9.7 inch, WiFi+LTE)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad6,7", "iPad Pro (12.9 inch, WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad6,8", "iPad Pro (12.9 inch, WiFi+LTE)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad6,11", "iPad (2017)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad6,12", "iPad (2017)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,1", "iPad Pro 2nd Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,2", "iPad Pro 2nd Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,3", "iPad Pro 10.5-inch 2nd Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,4", "iPad Pro 10.5-inch 2nd Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,5", "iPad 6th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,6", "iPad 6th Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,11", "iPad 7th Gen 10.2-inch (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad7,12", "iPad 7th Gen 10.2-inch (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,1", "iPad Pro 11 inch 3rd Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,2", "iPad Pro 11 inch 3rd Gen (1TB, WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,3", "iPad Pro 11 inch 3rd Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,4", "iPad Pro 11 inch 3rd Gen (1TB, WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,5", "iPad Pro 12.9 inch 3rd Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,6", "iPad Pro 12.9 inch 3rd Gen (1TB, WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,7", "iPad Pro 12.9 inch 3rd Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,8", "iPad Pro 12.9 inch 3rd Gen (1TB, WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,9", "iPad Pro 11 inch 4th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,10", "iPad Pro 11 inch 4th Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,11", "iPad Pro 12.9 inch 4th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad8,12", "iPad Pro 12.9 inch 4th Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad11,1", "iPad mini 5th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad11,2", "iPad mini 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad11,3", "iPad Air 3rd Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad11,4", "iPad Air 3rd Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad11,6", "iPad 8th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad11,7", "iPad 8th Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad12,1", "iPad 9th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad12,2", "iPad 9th Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad14,1", "iPad mini 6th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad14,2", "iPad mini 6th Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,1", "iPad Air 4th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,2", "iPad Air 4th Gen (WiFi+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,4", "iPad Pro 11 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,5", "iPad Pro 11 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,6", "iPad Pro 11 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,7", "iPad Pro 11 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,8", "iPad Pro 12.9 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,9", "iPad Pro 12.9 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,10", "iPad Pro 12.9 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,11", "iPad Pro 12.9 inch 5th Gen" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,16", "iPad Air 5th Gen (WiFi)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "iPad13,17", "iPad Air 5th Gen (WiFi+Cellular)" );

            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch1,1", "Apple Watch 38mm case" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch1,2", "Apple Watch 42mm  case" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch2,6", "Apple Watch Series 1 38mm case" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch2,7", "Apple Watch Series 1 42mm case" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch2,3", "Apple Watch Series 2 38mm case" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch2,4", "Apple Watch Series 2 42mm case" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch3,1", "Apple Watch Series 3 38mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch3,2", "Apple Watch Series 3 42mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch3,3", "Apple Watch Series 3 38mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch3,4", "Apple Watch Series 3 42mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch4,1", "Apple Watch Series 4 40mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch4,2", "Apple Watch Series 4 44mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch4,3", "Apple Watch Series 4 40mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch4,4", "Apple Watch Series 4 44mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,1", "Apple Watch Series 5 40mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,2", "Apple Watch Series 5 44mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,3", "Apple Watch Series 5 40mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,4", "Apple Watch Series 5 44mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,9", "Apple Watch 40mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,10", "Apple Watch 44mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,11", "Apple Watch 40mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch5,12", "Apple Watch 44mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,1", "Apple Watch 40mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,2", "Apple Watch 44mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,3", "Apple Watch 40mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,4", "Apple Watch 44mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,6", "Apple Watch 41mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,7", "Apple Watch 45mm case (GPS)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,8", "Apple Watch 41mm case (GPS+Cellular)" );
            RockMigrationHelper.AddDefinedValue( SystemGuid.DefinedType.APPLE_DEVICE_MODELS, "Watch6,9", "Apple Watch 45mm case (GPS+Cellular)" );

            Sql( @"MERGE INTO PersonalDevice pd
                       USING DefinedValue dv
                       ON pd.Model = dv.Value
                       WHEN MATCHED THEN 
                       UPDATE SET [model] = dv.Description;" );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            //
        }
    }
}
