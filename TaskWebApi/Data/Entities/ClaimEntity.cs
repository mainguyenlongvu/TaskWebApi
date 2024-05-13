namespace TaskWebApi.Repositories.Entities
{
    public class ClaimEntity
    {
        public int Id { get; set; }
        public List<UserClaimEntity> UserClaims { get; set; }

    }
}
