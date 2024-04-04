namespace Sales.Model.Branch
{
	public class EditVM
	{
		public Guid id { get; set; }

		public string? BranchName { get; set; }
		public List<IFormFile>? ListFileImg { get; set; }
	}
}
