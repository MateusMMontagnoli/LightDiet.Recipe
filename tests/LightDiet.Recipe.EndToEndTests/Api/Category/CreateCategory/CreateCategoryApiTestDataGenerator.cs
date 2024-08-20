namespace LightDiet.Recipe.EndToEndTests.Api.Category.CreateCategory;

public class CreateCategoryApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int quantityOfInputs = 10)
    {
        var fixture = new CreateCategoryApiTestFixture();

        var invalidInputList = new List<object[]>();

        var nameMinLengthException = "Name should be at least 3 characters long";

        var nameMaxLengthException = "Name should be less or equal than 50 characters long";

        var nameEmptyException = "Name should not be empty or null";

        var nameNullException = "Name should not be empty or null";

        var descriptionMaxLengthException = "Description should be less or equal than 150 characters long";

        var descriptionNullException = "Description should not be null";

        var totalInvalidCases = 5;

        for (int i = 0; i < quantityOfInputs; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    var invalidInputByNameMinLength = fixture.GetValidInput();

                    invalidInputByNameMinLength.Name = fixture.GetInvalidNameTooShort();

                    invalidInputList.Add(new object[]
                    {
                        invalidInputByNameMinLength,
                        nameMinLengthException
                    });
                    break;
                case 1:
                    {
                        var invalidInputByNameMaxLength = fixture.GetValidInput();

                        invalidInputByNameMaxLength.Name = fixture.GetInvalidNameTooLong();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByNameMaxLength,
                            nameMaxLengthException
                        });
                        break;
                    }
                case 2:
                    {
                        var invalidInputByNameEmpty = fixture.GetValidInput();

                        invalidInputByNameEmpty.Name = fixture.GetInvalidNameByEmptyName();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByNameEmpty,
                            nameEmptyException
                        });
                        break;
                    }
                case 3:
                    {
                        var invalidInputByNameNullity = fixture.GetValidInput();

                        invalidInputByNameNullity.Name = fixture.GetInvalidNameByNullName();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByNameNullity,
                            nameNullException
                        });
                        break;
                    }
                case 4:
                    {
                        var invalidInputByDescriptionMaxLength = fixture.GetValidInput();

                        invalidInputByDescriptionMaxLength.Description = fixture.GetInvalidDescriptionTooLong();

                        invalidInputList.Add(new object[]
                        {
                            invalidInputByDescriptionMaxLength,
                            descriptionMaxLengthException
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
