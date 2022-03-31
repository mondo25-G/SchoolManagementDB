Database.cs,
DatabaseFactory.cs,
DatabaseFactorySectionHandler.cs,
DataWorker.cs,
are the classes that form the factory method design pattern for the database.

SqlDatabase.cs 
is the actual database concrete class 

DataReaderDataHelper.cs
is a helper class for retrieve/print data for the DataReader class.

DuplicateDataHelper.cs,
StoredProcHelper.cs,
are helper classes for record duplication check/ stored procedures respectively
regarding the database schema that corresponds to the SchoolEntity class diagram.

SchoolManager.cs
is a DataWorker that manages CRUD operations
regarding the database schema that corresponds to the SchoolEntity class diagram.

