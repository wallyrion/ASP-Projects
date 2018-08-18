using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Eshop.Domain.Entities;
using Eshop.Domain.Entities.Goods;
using Eshop.Domain.Entities.Helpers;

namespace Eshop.Domain.Concrete
{
    public class ShopContextInitializer : DropCreateDatabaseAlways<ShopContext>
    {
        protected override void Seed(ShopContext context)
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Monitors",
                    Properties = new List<Property>
                    {
                        new Property
                        {
                            Name = "Manufacturer",
                            Specifications = new List<Specification>
                            {
                                new Specification {Name = "Acer"},
                                new Specification {Name = "Asus"},
                                new Specification {Name = "AOC"},
                                new Specification {Name = "Samsung"},
                            }
                        },
                        new Property
                        {
                            Name = "Screen Size",
                            Specifications = new List<Specification>
                            {
                                new Specification {Name = "21"},
                                new Specification {Name = "22"},
                                new Specification {Name = "24"},
                                new Specification {Name = "27"},
                            }
                        },
                        new Property
                        {
                            Name = "Resolution",
                            Specifications = new List<Specification>
                            {
                                new Specification {Name = "1920x1080"},
                                new Specification {Name = "1280x1024"},
                                new Specification {Name = "1024x768"},
                                new Specification {Name = "3840x2160"},
                            }
                        }

                    }
                },
                new Category
                {
                    Name = "CellPhones",
                    Properties = new List<Property>
                    {
                        new Property
                        {
                            Name = "Display Size",
                            Specifications = new List<Specification>
                            {
                                new Specification {Name = "4.5"},
                                new Specification {Name = "5.0"},
                                new Specification {Name = "5.5"},
                                new Specification {Name = "4.7"},
                                new Specification {Name = "5.7"},
                                new Specification {Name = "6.0"}
                            }
                        },
                        new Property
                        {
                            Name = "Color",
                            Specifications = new List<Specification>
                            {
                                new Specification {Name = "Black"},
                                new Specification {Name = "White"},
                                new Specification {Name = "Blue"},
                                new Specification {Name = "Green"},
                                new Specification {Name = "Gold"},
                                new Specification {Name = "Gray"}
                            }
                        },
                        new Property
                        {
                            Name = "ROM",
                            Specifications = new List<Specification>
                            {
                                new Specification {Name = "<2G"},
                                new Specification {Name = "2G"},
                                new Specification {Name = "4G"},
                                new Specification {Name = "8G"},
                                new Specification {Name = "16G"},
                                new Specification {Name = "32G"},
                                new Specification {Name = "64G"},
                                new Specification {Name = "128G"}
                            }
                        },
                        new Property
                        {
                            Name = "Manufacturer",
                            Specifications = new List<Specification>
                            {
                                new Specification {Name = "Xiaomi"},
                                new Specification {Name = "Huawei"},
                                new Specification {Name = "Apple"},
                                new Specification {Name = "Samsung"},
                                new Specification {Name = "LG"},
                                new Specification {Name = "HTC"},
                                new Specification {Name = "Lenovo"},

                            }
                        }
                    }

                }
            };


            categories.ForEach(category => context.Categories.Add(category));

            context.Users.Add(new User {Login = "alexey", Password = "alexey", Role = "Admin"});
            context.Users.Add(new User {Login = "admin", Password = "admin", Role = "Admin"});
            context.Users.Add(new User {Login = "moder", Password = "moder", Role = "Moderator"});


            var goods = new List<Good>();
            Good good;
            // monitors
            goods.Add(new Good
            {
                Name = "Acer R240HY bidx 23.8-Inch IPS HDMI DVI VGA (1920 x 1080) Widescreen Monitor",
                Price = 129

            });
            AddSpecificationToGood(categories, "Monitors", goods[0], "Manufacturer", "Acer");
            AddSpecificationToGood(categories, "Monitors", goods[0], "Resolution", "1920x1080");
            AddSpecificationToGood(categories, "Monitors", goods[0], "Screen Size", "27");
            goods.Add(new Good
            {
                Name = "ASUS VS248H-P 24 Full HD 1920x1080 2ms HDMI DVI VGA Back-lit LED Monitor",
                Price = 116
            });
            AddSpecificationToGood(categories, "Monitors", goods[1], "Manufacturer", "Asus");
            AddSpecificationToGood(categories, "Monitors", goods[1], "Resolution", "1920x1080");
            AddSpecificationToGood(categories, "Monitors", goods[1], "Screen Size", "24");

            //cell phones
            //cell phone1
            good = new Good
            {
                Name = "Apple iPhone 6s Dual Core 2GB RAM 16GB 64GB 128GB ROM 4.7' 12.0MP",
                Price = 220
            };
            AddSpecificationToGood(categories, "CellPhones", good, "Manufacturer", "Apple");
            AddSpecificationToGood(categories, "CellPhones", good, "Display Size", "4.7");
            AddSpecificationToGood(categories, "CellPhones", good, "ROM", "8G");
            AddSpecificationToGood(categories, "CellPhones", good, "Color", "Black");
            goods.Add(good);
            //cell phone 2
            good = new Good
            {
                Name = "Samsung Galaxy S9 Single SIM 64GB SM-G9600 Factory Unlocked 4G Smartphone (Midnight Black) -International Version",
                Price = 634
            };
            AddSpecificationToGood(categories, "CellPhones", good, "Manufacturer", "Samsung");
            AddSpecificationToGood(categories, "CellPhones", good, "Display Size", "6");
            AddSpecificationToGood(categories, "CellPhones", good, "ROM", "128G");
            AddSpecificationToGood(categories, "CellPhones", good, "Color", "Black");
            goods.Add(good);
            //cell phone 3
            good = new Good
            {
                Name = "Redmi 5 (Gold, 16GB)",
                Price = 160
            };
            AddSpecificationToGood(categories, "CellPhones", good, "Manufacturer", "Xiaomi");
            AddSpecificationToGood(categories, "CellPhones", good, "Display Size", "5.7");
            AddSpecificationToGood(categories, "CellPhones", good, "ROM", "16G");
            AddSpecificationToGood(categories, "CellPhones", good, "Color", "Gold");
            goods.Add(good);
            //cell phone 3
            good = new Good
            {
                Name = "Xiaomi MI A1 (64GB, 4GB RAM) with Android One & Dual Cameras, 5.5' Dual SIM Unlocked, Global GSM Version, No Warranty (Black)",
                Price = 199
            };
            AddSpecificationToGood(categories, "CellPhones", good, "Manufacturer", "Xiaomi");
            AddSpecificationToGood(categories, "CellPhones", good, "Display Size", "5.5");
            AddSpecificationToGood(categories, "CellPhones", good, "ROM", "64G");
            AddSpecificationToGood(categories, "CellPhones", good, "Color", "Black");
            goods.Add(good);


             goods.ForEach(g => context.Goods.Add(g));

          




        }

        private void AddSpecificationToGood(List<Category> categories, string category, Good good, string property, string specification)
        {

            var spec = (categories.FirstOrDefault(cat => cat.Name == category))?.Properties
                .FirstOrDefault(prop => prop.Name == property)
                ?.Specifications
                .FirstOrDefault(s => s.Name == specification);
            
            if (spec != null)
            {
                good.Specifications.Add(spec);
                good.Category = spec.Property.Category;
            }

        }

    }
}

