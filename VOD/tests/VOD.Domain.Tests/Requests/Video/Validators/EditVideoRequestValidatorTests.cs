namespace VOD.Domain.Tests.Requests.Video.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using VOD.Domain.Requests.Video;
    using VOD.Fixtures.Data;
    using Xunit;
    using FluentValidation.TestHelper;
    using VOD.Domain.Requests.Video.Validators;
    using VOD.Domain.Services;
    using Moq;
    using VOD.Domain.Requests.Kind;
    using VOD.Domain.Requests.Genre;

    public class EditVideoRequestValidatorTests
    {
        public EditVideoRequestValidatorTests()
        {
            this.KindServiceMock = new Mock<IKindService>();
            this.GenreServiceMock = new Mock<IGenreService>();
            this.Validator = new EditVideoRequestValidator(GenreServiceMock.Object, KindServiceMock.Object);
        }

        private EditVideoRequestValidator Validator { get; set; }

        private Mock<IKindService> KindServiceMock { get; set; }

        private Mock<IGenreService> GenreServiceMock { get; set; }

        [Fact]
        public void Should_have_error_when_Id_is_empty()
        {
            EditVideoRequest addVideoRequest = TestDataFactory.CreateEditVideoRequest();
            addVideoRequest.Id = Guid.Empty;

            Validator.ShouldHaveValidationErrorFor(x => x.Id, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_KindId_is_null()
        {
            EditVideoRequest addVideoRequest = TestDataFactory.CreateEditVideoRequest();
            addVideoRequest.KindId = Guid.Empty;

            Validator.ShouldHaveValidationErrorFor(x => x.KindId, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_GenreId_is_null()
        {
            EditVideoRequest addVideoRequest = TestDataFactory.CreateEditVideoRequest();
            addVideoRequest.GenreId = Guid.Empty;

            Validator.ShouldHaveValidationErrorFor(x => x.GenreId, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_Duration_is_greater_than_65535()
        {
            EditVideoRequest addVideoRequest = TestDataFactory.CreateEditVideoRequest();
            addVideoRequest.Duration = 65536;

            Validator.ShouldHaveValidationErrorFor(x => x.Duration, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_Duration_is_lower_than_0()
        {
            EditVideoRequest addVideoRequest = TestDataFactory.CreateEditVideoRequest();
            addVideoRequest.Duration = -1;

            Validator.ShouldHaveValidationErrorFor(x => x.Duration, addVideoRequest);
        }

        [Fact]
        public void Should_not_have_error_when_Duration_is_null()
        {
            EditVideoRequest addVideoRequest = TestDataFactory.CreateEditVideoRequest();
            addVideoRequest.Duration = null;

            Validator.ShouldNotHaveValidationErrorFor(x => x.Duration, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_KindId_doesnt_exist()
        {
            KindServiceMock
                .Setup(x => x.GetKindAsync(It.IsAny<GetKindRequest>()))
                .ReturnsAsync(() => null);

            EditVideoRequest addVideoRequest = new EditVideoRequest { KindId = Guid.NewGuid() };

            Validator.ShouldHaveValidationErrorFor(x => x.KindId, addVideoRequest);
        }

        [Fact]
        public void Should_have_error_when_GenreId_doesnt_exist()
        {
            GenreServiceMock
                .Setup(x => x.GetGenreAsync(It.IsAny<GetGenreRequest>()))
                .ReturnsAsync(() => null);

            EditVideoRequest addVideoRequest = new EditVideoRequest { GenreId = Guid.NewGuid() };

            Validator.ShouldHaveValidationErrorFor(x => x.GenreId, addVideoRequest);
        }
    }
}
