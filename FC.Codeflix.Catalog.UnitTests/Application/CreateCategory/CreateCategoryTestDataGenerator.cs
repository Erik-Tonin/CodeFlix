﻿namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    public class CreateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new CreateCategoryTestFixture();
            var invalidInputs = new List<object[]>();
            var totalInvalidCases = 4;

            for(int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputs.Add(new object[]
                        {
                            fixture.GetInvalidInputShortName(),
                            "Name should be at leats 3 characters long"
                        });
                        break;
                    case 1:
                        invalidInputs.Add(new object[]
                        {
                            fixture.GetInvalidInputTooLongName(),
                            "Name should be less or equal 255 characters long"
                        });
                        break;
                    case 2:
                        invalidInputs.Add(new object[]
                        {
                            fixture.GetInvalidInputCategoryNull(),
                            "Description should not be null"
                        });
                        break;
                    case 3:
                        invalidInputs.Add(new object[]
                        {
                            fixture.GetInvalidInputTooLongDescription(),
                            "Description should be less or equal 10000 characters long"
                        });
                        break;
                    default:
                        break;
                }
            }        
            return invalidInputs;
        }
    }
}