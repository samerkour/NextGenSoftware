namespace NextGen.Modules.Identity.ClaimGroups.Dtos
{
    public class ClaimGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
