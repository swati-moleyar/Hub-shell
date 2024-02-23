using System;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Mapping;
using Hub.Shell.External.Hub.Resources;
using Xunit;

namespace Hub.Shell.Domain.Test.Mapping
{
    public class AppsMappingTests
    {
        [Fact]
        public void Map_Application_To_Contract()
        {
            var mockResource = new ApplicationResource
            {
                Id = "manifests",
                Title = "Manifests",
                Href = "/#Manifests/",
                Description = "Import inventory from your traceability provider"
            };

            var result = mockResource.ToContract();

            result.Should().BeEquivalentTo(new ApplicationContract
            {
                Id = "manifests",
                Name = "Manifests",
                Href = "/#Manifests/",
                Description = "Import inventory from your traceability provider",
                Version = 1
            });
        }

        [Fact]
        public void Map_Group_To_Contract()
        {
            var mockResource = new ApplicationGroupResource
            {
                GroupName = "Endless Aisle",
                Icon = "shelf",
                Path = "#",
                DefaultApp = "endlessAisleDevices",
                Applications = new string[] {"endlessAisleDevices", "endlessAisleConfigurations"}
            };

            var result = mockResource.ToContract(new List<ApplicationResource> {
                new ApplicationResource { Id = "endlessAisleDevices", Title = "Devices" },
                new ApplicationResource { Id = "endlessAisleConfigurations", Title = "Configurations" }
            }, "");

            result.Should().BeEquivalentTo(new ApplicationGroupContract
            {
                Name = "Endless Aisle",
                Icon = "shelf",
                DefaultApp = null,
                Apps = new List<ApplicationContract> { 
                    new ApplicationContract { Id = "endlessAisleConfigurations", Name = "Configurations", Version = 1 },
                    new ApplicationContract { Id = "endlessAisleDevices", Name = "Devices", Version = 1 }
                }
            }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Map_Group_To_Contract_Applies_Domain_Rebranding()
        {
            var mockResource = new ApplicationGroupResource
            {
                GroupName = "Endless Aisle",
                Icon = "shelf",
                Path = "#",
                DefaultApp = "endlessAisleDevices",
                Applications = new string[] {"endlessAisleDevices", "endlessAisleConfigurations"},
                DomainRebrand = new Dictionary<string, string> {{ "covasoft", "Digital Menus" }}
            };

            var result = mockResource.ToContract(new List<ApplicationResource> {
                new ApplicationResource { Id = "endlessAisleDevices" },
                new ApplicationResource { Id = "endlessAisleConfigurations" }
            }, "covasoft");

            result.Name.Should().Be("Digital Menus");
        }

        [Fact]
        public void Map_Group_To_Contract_Sets_DefaultApp_With_DisableToggle()
        {
            var mockResource = new ApplicationGroupResource
            {
                GroupName = "Endless Aisle",
                Icon = "shelf",
                Path = "#",
                DefaultApp = "endlessAisleDevices",
                Applications = new string[] {"endlessAisleDevices", "endlessAisleConfigurations"},
                DisableToggle = true
            };

            var result = mockResource.ToContract(new List<ApplicationResource> {
                new ApplicationResource { Id = "endlessAisleDevices" },
                new ApplicationResource { Id = "endlessAisleConfigurations" }
            }, "");

            result.DefaultApp.Should().BeEquivalentTo(new ApplicationContract { Id = "endlessAisleDevices", Version = 1 });
        }

        [Fact]
        public void Map_Group_To_Contract_Without_Apps_Returns_Null()
        {
            var mockResource = new ApplicationGroupResource
            {
                GroupName = "Endless Aisle",
                Icon = "shelf",
                Path = "#",
                DefaultApp = "endlessAisleDevices",
                Applications = new string[] {"endlessAisleDevices", "endlessAisleConfigurations"}
            };

            var result = mockResource.ToContract(new List<ApplicationResource>(), "");

            result.Should().BeNull();
        }

        [Fact]
        public void Map_Group_To_Contract_Excludes_Unincluded_Apps()
        {
            var mockResource = new ApplicationGroupResource
            {
                GroupName = "Endless Aisle",
                Icon = "shelf",
                Path = "#",
                DefaultApp = "endlessAisleDevices",
                Applications = new string[] {"endlessAisleDevices", "endlessAisleConfigurations", "someOtherApp"}
            };

            var result = mockResource.ToContract(new List<ApplicationResource> {
                new ApplicationResource { Id = "endlessAisleDevices" },
                new ApplicationResource { Id = "endlessAisleConfigurations" }
            }, "");

            result.Apps.Should().NotContain(x => x.Id == "someOtherApp");
        }

        [Fact]
        public void Map_Group_To_Contract_Applies_UseDefaultOrder()
        {
            var mockResource = new ApplicationGroupResource
            {
                GroupName = "Endless Aisle",
                Icon = "shelf",
                Path = "#",
                DefaultApp = "endlessAisleDevices",
                Applications = new string[] {"endlessAisleDevices", "endlessAisleConfigurations"},
                UseDefaultOrder = true
            };

            var result = mockResource.ToContract(new List<ApplicationResource> {
                new ApplicationResource { Id = "endlessAisleConfigurations", Title = "Configurations" },
                new ApplicationResource { Id = "endlessAisleDevices", Title = "Devices" }
            }, "");

            result.Apps.Should().BeEquivalentTo(new List<ApplicationContract> { 
                new ApplicationContract { Id = "endlessAisleDevices", Name = "Devices", Version = 1 },
                new ApplicationContract { Id = "endlessAisleConfigurations", Name = "Configurations", Version = 1 }
            }, options => options.WithStrictOrdering());
        }
    }
}