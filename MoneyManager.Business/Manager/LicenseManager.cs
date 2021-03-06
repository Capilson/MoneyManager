﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using MoneyManager.Foundation;
using Xamarin;

namespace MoneyManager.Business.Manager {
    public class LicenseManager {
        public readonly string FeaturepackProductKey = "10001";
        private bool _isFeaturepackLicensed;

        public bool IsFeaturepackLicensed {
            get {
                CheckLicenceFeaturepack();
                return _isFeaturepackLicensed;
            }
        }

        public async Task CheckLicenceFeaturepack() {
#if DEBUG
            _isFeaturepackLicensed = true;
            return;
#endif
            try {
                var listing = await CurrentApp.LoadListingInformationAsync();
                var featurepackLicence =
                    listing.ProductListings.FirstOrDefault(p => p.Value.ProductId == FeaturepackProductKey);

                if (CurrentApp.LicenseInformation.ProductLicenses != null) {
                    _isFeaturepackLicensed =
                        CurrentApp.LicenseInformation.ProductLicenses[featurepackLicence.Key].IsActive;
                }
            }
            catch (Exception ex) {
                if (!ex.Message.Contains("0x805A0194")) {
                    InsightHelper.Report(ex);
                }
            }
        }
    }
}