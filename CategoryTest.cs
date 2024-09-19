using System;
using Xunit;

public class CategoryTest
{
	public CategoryTest()
	{
		[Fact()]
		public void Instantiate()
		//Arrange
		var validData = new 
		{
			Name = "category name",
			Description = "category description"
		}
		//Act
		var category = new Category(validData.Name, validData.Description);
		//Assert
		Assert.NotNull(category);
		Assert.Equal(validData.Name, validData.Description);
		Assert.Equal(validData.Description, validData.Description);
    }
}
