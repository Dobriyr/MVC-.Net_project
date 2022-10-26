using System;
using System.Collections.Generic;
using System.Text;
using Business.Models;

namespace Business.Validation
{
    public static class Validation
    {
        public static void IsValid(this CustomerModel model)
        {
            if (model == null)
                throw new MarketException("customer is null");
            if (model.Surname == string.Empty || model.Surname == null)
                throw new MarketException("surname is empty");
            if (model.Name == string.Empty || model.Name == null)
                throw new MarketException("name is empty");
            if (model.DiscountValue < 0)
                throw new MarketException("discount value is not valid");
            if (model.BirthDate > DateTime.Now || model.BirthDate < DateTime.Now.AddYears(-100))
                throw new MarketException("birthdate is invalid");
        }

        public static void IsValid(this ProductModel model) {
            if (model is null)
            {
                throw new MarketException("ProductModel value is null");
            }
            if (model.ProductName == string.Empty)
            {
                throw new MarketException("ProductModel value ProductName is empty");
            }
            if (model.Price < 0)
            {
                throw new MarketException("ProductModel value Price out of range");
            }
        }
        
        public static void IsValid(this CustomerActivityModel model) {
            if (model == null) {
                throw new MarketException("customerActivityModel is null");
            }

            if (model.ReceiptSum < 0) {
                throw new MarketException("Receipt sum is < 0");
            }

        }
        public static void IsValid(this FilterSearchModel model) {
            if (model == null)
                throw new MarketException("FilterSearchModel is null");    
        }
        public static void IsValid(this ProductCategoryModel model)
        {
            if (model == null)
                throw new MarketException("ProductCategoryModel is null");
        }
    }
}
