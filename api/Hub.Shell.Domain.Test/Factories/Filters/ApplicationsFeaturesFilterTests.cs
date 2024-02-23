using System.Collections.Generic;
using Hub.Shell.Domain.Factories.Filters;
using Hub.Shell.External.FeatureToggles;
using Hub.Shell.External.FeatureToggles.Resources;
using Hub.Shell.External.Hub.Resources;
using Functional;
using FluentAssertions;
using NSubstitute;
using Xunit;


namespace Hub.Shell.Domain.Test.Factories.Filters
{
    public class ApplicationsFeaturesFilterTests
    {
        private readonly IFeatureTogglesServiceClient _fakeFeatureToggles;

        private readonly ApplicationsFeaturesFilter _sut;

        private const int EntityId = 456;

        public ApplicationsFeaturesFilterTests()
        {
            _fakeFeatureToggles = Substitute.For<IFeatureTogglesServiceClient>();

            _sut = new ApplicationsFeaturesFilter(_fakeFeatureToggles);
        }

        [Fact]
        public async void Filters_By_Feature_Toggles()
        {
            var mockApplications = new List<ApplicationResource>
            {
                new ApplicationResource
                {
                    Id = "NullFeatureToggles",
                    FeatureToggles = null
                },
                new ApplicationResource
                {
                    Id = "HasFeatureToggles",
                    FeatureToggles = new string[] { "FeatureToggle1", "FeatureToggle1;Subfeature", "FeatureToggle2" }
                },
                new ApplicationResource
                {
                    Id = "MissingFeatureToggles",
                    FeatureToggles = new string[] { "FeatureToggle1", "MissingFeatureToggle" }
                }
            };

            _fakeFeatureToggles.GetEnabledFeatureToggles(EntityId).Returns(Result.Success<IEnumerable<FeatureToggleResource>, FeatureTogglesError>
            (
                new List<FeatureToggleResource>
                {
                    new FeatureToggleResource
                    {
                        NameKey = "FeatureToggle1",
                        SubFeatures = new List<FeatureToggleResource>
                        {
                            new FeatureToggleResource
                            {
                                NameKey = "FeatureToggle1;Subfeature"
                            }
                        }
                    },
                    new FeatureToggleResource
                    {
                        NameKey = "FeatureToggle2"
                    }
                }
            ));

            var result = await _sut.Apply(mockApplications, EntityId);

            result.Success().ValueOrDefault().Should().BeEquivalentTo(new List<ApplicationResource>
            {
                new ApplicationResource
                {
                    Id = "NullFeatureToggles",
                    FeatureToggles = null
                },
                new ApplicationResource
                {
                    Id = "HasFeatureToggles",
                    FeatureToggles = new string[] { "FeatureToggle1", "FeatureToggle1;Subfeature", "FeatureToggle2" }
                }
            });
        }
    }
}
