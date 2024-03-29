﻿namespace Example.Domain.Entities;

public interface ICreatedModifiedEntityFields
{
    string CreatedBy { get; set; }
    DateTime CreatedDate { get; set; }
    string ModifiedBy { get; set; }
    DateTime ModifiedDate { get; set; }
}
