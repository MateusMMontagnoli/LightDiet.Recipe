﻿namespace LightDiet.Recipe.IntegrationTests.Application.UseCases.Category.CreateCategory;

public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidCategories(int quantityOfInputs = 12)
    {
        var fixture = new CreateCategoryTestFixture();

        var invalidInputList = new List<object[]>();

        var nameMinLengthException = "Name should be at least 3 characters long";

        var nameMaxLengthException = "Name should be less or equal than 50 characters long";

        var nameEmptyException = "Name should not be empty or null";

        var nameNullException = "Name should not be empty or null";

        var descriptionMaxLengthException = "Description should be less or equal than 150 characters long";

        var descriptionNullException = "Description should not be null";

        var totalInvalidCases = 6;

        for (int i = 0; i < quantityOfInputs; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    var invalidInputByNameMinLength = fixture.GetInvalidInputByShortName();

                    invalidInputList.Add(new object[]
                    {
                        invalidInputByNameMinLength,
                        nameMinLengthException
                    });
                    break;
                case 1:
                    {
                        var invalidInputByNameMaxLength = fixture.GetInvalidInputByGreaterName();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByNameMaxLength,
                            nameMaxLengthException
                        });
                        break;
                    }
                case 2:
                    {
                        var invalidInputByNameEmpty = fixture.GetInvalidInputByEmptyName();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByNameEmpty,
                            nameEmptyException
                        });
                        break;
                    }
                case 3:
                    {
                        var invalidInputByNameNullity = fixture.GetInvalidInputByNullName();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByNameNullity,
                            nameNullException
                        });
                        break;
                    }
                case 4:
                    {
                        var invalidInputByDescriptionMaxLength = fixture.GetInvalidInputByGreaterDescription();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByDescriptionMaxLength,
                            descriptionMaxLengthException
                        });
                        break;
                    }
                case 5:
                    {
                        var invalidInputByDescriptionNullity = fixture.GetInvalidInputByNullDescription();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByDescriptionNullity,
                            descriptionNullException
                        });

                        break;
                    }

                default:
                    break;
            }
        }

        return invalidInputList;
    }
}
