namespace MoviesApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artist",
                c => new
                    {
                        ArtistID = c.Int(nullable: false, identity: true),
                        ArtistLastName = c.String(),
                        ArtistFirstName = c.String(),
                    })
                .PrimaryKey(t => t.ArtistID);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MoviesID = c.Int(nullable: false, identity: true),
                        MovieTitle = c.String(),
                        Rate = c.Int(),
                        MovieReleaseDate = c.DateTime(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MoviesID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserLastname = c.String(),
                        UserFirstname = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.MoviesArtist",
                c => new
                    {
                        Movies_MoviesID = c.Int(nullable: false),
                        Artist_ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movies_MoviesID, t.Artist_ArtistID })
                .ForeignKey("dbo.Movies", t => t.Movies_MoviesID, cascadeDelete: true)
                .ForeignKey("dbo.Artist", t => t.Artist_ArtistID, cascadeDelete: true)
                .Index(t => t.Movies_MoviesID)
                .Index(t => t.Artist_ArtistID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Movies", "UserID", "dbo.User");
            DropForeignKey("dbo.MoviesArtist", "Artist_ArtistID", "dbo.Artist");
            DropForeignKey("dbo.MoviesArtist", "Movies_MoviesID", "dbo.Movies");
            DropIndex("dbo.MoviesArtist", new[] { "Artist_ArtistID" });
            DropIndex("dbo.MoviesArtist", new[] { "Movies_MoviesID" });
            DropIndex("dbo.Movies", new[] { "UserID" });
            DropTable("dbo.MoviesArtist");
            DropTable("dbo.User");
            DropTable("dbo.Movies");
            DropTable("dbo.Artist");
        }
    }
}
