namespace VOD.Domain.Tests.Requests.Video.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xunit;
    using FluentValidation.TestHelper;
    using VOD.Domain.Requests.Video.Validators;
    using VOD.Domain.Requests.Video;
    using VOD.Fixtures.Data;
    using Moq;
    using VOD.Domain.Services;
    using VOD.Domain.Requests.Genre;
    using VOD.Domain.Responses;
    using VOD.Domain.Requests.Kind;

    public class AddVideoRequestValidatorTests
    {
        public AddVideoRequestValidatorTests()
        {
            this.KindServiceMock = new Mock<IKindService>();
            this.GenreServiceMock = new Mock<IGenreService>();
            this.Validator = new AddVideoRequestValidator(GenreServiceMock.Object, KindServiceMock.Object);
        }

        private AddVideoRequestValidator Validator { get; set; }

        private Mock<IKindService> KindServiceMock { get; set; }

        private Mock<IGenreService> GenreServiceMock { get; set; }

        [Fact]
        public void Should_have_error_when_KindId_is_empty()
        {
            AddVideoRequest addVideoRequest = TestDataFactory.CreateAddVideoRequest();
            addVideoRequest.KindId = Guid.Empty;

            Validator.ShouldHaveValidationErrorFor(x => x.KindId, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_GenreId_is_empty()
        {
            AddVideoRequest addVideoRequest = TestDataFactory.CreateAddVideoRequest();
            addVideoRequest.GenreId = Guid.Empty;

            Validator.ShouldHaveValidationErrorFor(x => x.GenreId, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_Duration_is_greater_than_65535()
        {
            AddVideoRequest addVideoRequest = TestDataFactory.CreateAddVideoRequest();
            addVideoRequest.Duration = 65536;

            Validator.ShouldHaveValidationErrorFor(x => x.Duration, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_Duration_is_lower_than_0()
        {
            AddVideoRequest addVideoRequest = TestDataFactory.CreateAddVideoRequest();
            addVideoRequest.Duration = -1;

            Validator.ShouldHaveValidationErrorFor(x => x.Duration, addVideoRequest);
        }

        [Fact]
        public void Should_not_have_error_when_Duration_is_null()
        {
            AddVideoRequest addVideoRequest = TestDataFactory.CreateAddVideoRequest();
            addVideoRequest.Duration = null;

            Validator.ShouldNotHaveValidationErrorFor(x => x.Duration, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_KindId_doesnt_exist()
        {
            KindServiceMock
                .Setup(x => x.GetKindAsync(It.IsAny<GetKindRequest>()))
                .ReturnsAsync(() => null);

            AddVideoRequest addVideoRequest = new AddVideoRequest { KindId = Guid.NewGuid() };

            Validator.ShouldHaveValidationErrorFor(x => x.KindId, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_GenreId_doesnt_exist()
        {
            GenreServiceMock
                .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>()))
                .ReturnsAsync(() => null);

            AddVideoRequest addVideoRequest = new AddVideoRequest { GenreId = Guid.NewGuid() };

            Validator.ShouldHaveValidationErrorFor(x => x.GenreId, addVideoRequest);
        }
    }
}
