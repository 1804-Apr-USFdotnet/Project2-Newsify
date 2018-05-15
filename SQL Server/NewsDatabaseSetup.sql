-- This Query is to create a new database for Project 2
-- The database will store links for news articles, user information, and user comments
-- for the news articles.
create database NewsDB;
Go;

-- Create a new schema for the database
create schema News;
Go;

-- Create a new table for storing the different Sources
create table News.Sources
(
	[PK] int primary key identity(1,1),
	[Id] varchar(50) null, -- Source's ID; can be null
	[Name] varchar(50) not null, -- Name of the source; can't be null
	[Description] varchar(200) not null, -- Description of the source
	[Url] varchar(200) not null, -- Link to the main page of the source
	[Category] varchar(50) not null, -- Type of articles the sources has
	[Langauge] varchar(5) not null, -- The langauge the articles are in
	[Country] varchar(5) not null -- Country the source is from
);
Go;

-- Create a new table for storing links for news articles and their relevant information
create table News.Articles
(
	[ID] int primary key identity(1,1), -- Primary key autoincrements
	[Source] int foreign key references News.Sources(PK), -- FK to Source
	[Author] varchar(50) null, -- Author of the article; can be null
	[Title] varchar(100) not null, -- Title of the article; can't be null
	[Description] varchar(500) not null, -- Description for the article; can't be null
	[Url] varchar(200) not null, -- Link to the article's original location; can't be null
	[UrlToImage] varchar(200) not null, -- Link to the Iamge used for the article; can't be null
	[PublishAt] datetime not null, -- The time the article was published; can't be null
	[Category] varchar(50) not null, -- The category the article falls under; can't be null
	[Topic] varchar(50) null, -- Keyword for the article; can be null
	--[Country] varchar(100) not null, -- Country of origin of the article; can't be null
	--[Langauge] varchar(5) not null, -- should be only two characters to indicate the langauage
	[Active] bit not null -- Indicates if the article is active;
);
Go;

-- Create a new table for storing the type of user
create table News.UserTypes
(
	[ID] int primary key identity(1,1), -- primary key
	[UserType] varchar(50) not null -- name of the user type; can only be Admin, Mod, or User
);
Go;

-- Create a new table for storing user information
create table News.Users
(
	[ID] int primary key identity(1,1), -- primary key
	[UserName] varchar(50) unique not null, -- User's online display name; can't be null and has to be unique
	[Password] varchar(50) not null, -- User's account password; can't be null
	[FirstName] varchar(50) not null, -- User's First Name; can't be null
	[LastName] varchar(50) not null, -- User's Last Name; can't be null
	[BirthDate] date not null, -- User's Birth Date; can't be null
	[Type] int foreign key references News.UserTypes(ID) not null, -- Specifies what type the user is
	[Active] bit not null -- Indicates whether or not the user account is active or not 
);
Go;

-- Create a new table to store user comments along with the FK to the user's PK in User table
create table News.Comments
(
	[ID] int primary key identity(1,1), -- primary key
	--[UserID] int foreign key references News.Users(ID) not null, -- Foreign key; points to the user that made the comment
	[CommentedAt] datetime not null, -- The time that the comment was created
	[Comment] varchar(250) not null, -- The actual comment made
	--[CommentID] int null, -- indicates this comment is a response to the given comment
	[Modified] datetime not null, -- time the comment was edited; its the same as CommentAt if never edited
	--[Likes] int null, -- number of likes the comment has gotten
	--[Hates] int null -- number of dislikes the comment has gotten 
	[Active] bit not null -- Indicates whether or not the comment is active or not; deactive means it's deleted
);
Go;

-- Create a new table to store FKs for new articles and thier comments
create table News.Posts
(
	[ID] int primary key identity(1,1), -- primary key
	[ArticleID] int foreign key references News.Articles(ID) not null, -- FK to Article
	[CommentID] int foreign key references News.Comments(ID) not null, -- FK to Comment
	[UserID] int foreign key references News.Users(ID) not null, -- FK to User
	[ReplyTo] int foreign key references News.Comments(ID) null -- FK to Comment that this post a reply to
);
Go;