CREATE TABLE Newsletter (
  [Id] [INT] IDENTITY(1,1) NOT NULL,
  [Type] [nvarchar](50), 
  [Name] [nvarchar](500),
  [Url] [nvarchar](1000),
  [About] [nvarchar](MAX)
);

ALTER TABLE [Newsletter]
ADD PRIMARY KEY (Id);
