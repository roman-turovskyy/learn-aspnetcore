using Example.Domain.Common;

namespace Example.Domain.Enums;

public enum BlogStatus
{
    [ReferenceId("A087C3A4-0DF1-4F8A-A3E3-45F639D92071")]
    Draft,

    [ReferenceId("B75162B7-A85D-4F90-8508-A2F3CA7DE534")]
    OnReview,

    [ReferenceId("9CF41B5A-FD41-4624-8042-C1C5D6BEF4B5")]
    Published
}
