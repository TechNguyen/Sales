namespace Sales.Model.Comments
{
	public class CreateVM
	{
		public Guid? UserId { get; set; }
		public Guid? ProductId { get; set; }
		public string Comment { get; set; }
	}
}
