-- RentAPlace Database Schema
-- Created for ASP.NET Core Web API with Entity Framework Core

-- Create Database
CREATE DATABASE RentAPlaceDB;
GO

USE RentAPlaceDB;
GO

-- Users Table (for both Renters and Owners)
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20),
    UserType NVARCHAR(10) NOT NULL CHECK (UserType IN ('Renter', 'Owner')),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Property Types Table
CREATE TABLE PropertyTypes (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1
);

-- Property Features Table
CREATE TABLE PropertyFeatures (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1
);

-- Properties Table
CREATE TABLE Properties (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    OwnerId UNIQUEIDENTIFIER NOT NULL,
    PropertyTypeId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    State NVARCHAR(50) NOT NULL,
    Country NVARCHAR(50) NOT NULL,
    PostalCode NVARCHAR(20),
    Latitude DECIMAL(10, 8),
    Longitude DECIMAL(11, 8),
    PricePerNight DECIMAL(10, 2) NOT NULL,
    MaxGuests INT NOT NULL DEFAULT 1,
    Bedrooms INT NOT NULL DEFAULT 1,
    Bathrooms INT NOT NULL DEFAULT 1,
    SquareFootage INT,
    IsActive BIT NOT NULL DEFAULT 1,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (OwnerId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (PropertyTypeId) REFERENCES PropertyTypes(Id)
);

-- Property Images Table
CREATE TABLE PropertyImages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PropertyId UNIQUEIDENTIFIER NOT NULL,
    ImagePath NVARCHAR(500) NOT NULL,
    ImageName NVARCHAR(255) NOT NULL,
    IsPrimary BIT NOT NULL DEFAULT 0,
    DisplayOrder INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PropertyId) REFERENCES Properties(Id) ON DELETE CASCADE
);

-- Property Features Junction Table
CREATE TABLE PropertyPropertyFeatures (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PropertyId UNIQUEIDENTIFIER NOT NULL,
    PropertyFeatureId UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (PropertyId) REFERENCES Properties(Id) ON DELETE CASCADE,
    FOREIGN KEY (PropertyFeatureId) REFERENCES PropertyFeatures(Id) ON DELETE CASCADE,
    UNIQUE(PropertyId, PropertyFeatureId)
);

-- Reservations Table
CREATE TABLE Reservations (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PropertyId UNIQUEIDENTIFIER NOT NULL,
    RenterId UNIQUEIDENTIFIER NOT NULL,
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL,
    TotalNights INT NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Confirmed', 'Cancelled', 'Completed')),
    SpecialRequests NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PropertyId) REFERENCES Properties(Id) ON DELETE CASCADE,
    FOREIGN KEY (RenterId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Messages Table
CREATE TABLE Messages (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PropertyId UNIQUEIDENTIFIER NOT NULL,
    SenderId UNIQUEIDENTIFIER NOT NULL,
    ReceiverId UNIQUEIDENTIFIER NOT NULL,
    Subject NVARCHAR(200),
    Content NVARCHAR(MAX) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PropertyId) REFERENCES Properties(Id) ON DELETE CASCADE,
    FOREIGN KEY (SenderId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (ReceiverId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Reviews Table
CREATE TABLE Reviews (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PropertyId UNIQUEIDENTIFIER NOT NULL,
    RenterId UNIQUEIDENTIFIER NOT NULL,
    ReservationId UNIQUEIDENTIFIER NOT NULL,
    Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PropertyId) REFERENCES Properties(Id) ON DELETE CASCADE,
    FOREIGN KEY (RenterId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (ReservationId) REFERENCES Reservations(Id) ON DELETE CASCADE,
    UNIQUE(PropertyId, RenterId, ReservationId)
);

-- Notifications Table
CREATE TABLE Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Title NVARCHAR(100) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Insert default data
INSERT INTO PropertyTypes (Name, Description) VALUES
('Apartment', 'A self-contained housing unit in a larger building'),
('House', 'A single-family dwelling'),
('Villa', 'A large, luxurious house, often in a rural or suburban setting'),
('Condo', 'A privately owned individual unit in a multi-unit building'),
('Studio', 'A small apartment with a combined living and sleeping area'),
('Loft', 'A large, open space converted from industrial use'),
('Townhouse', 'A multi-story house sharing walls with adjacent properties');

INSERT INTO PropertyFeatures (Name, Description) VALUES
('WiFi', 'High-speed internet access'),
('Pool', 'Swimming pool available'),
('Garden', 'Outdoor garden space'),
('Beach Access', 'Direct access to beach'),
('Parking', 'Parking space available'),
('Kitchen', 'Fully equipped kitchen'),
('Air Conditioning', 'Air conditioning system'),
('Heating', 'Heating system'),
('Pet Friendly', 'Pets are allowed'),
('Smoking Allowed', 'Smoking is permitted'),
('Balcony', 'Private balcony or terrace'),
('Fireplace', 'Fireplace available'),
('Gym', 'Fitness center or gym'),
('Spa', 'Spa or wellness facilities'),
('Ocean View', 'View of the ocean'),
('Mountain View', 'View of mountains'),
('City View', 'View of the city'),
('Lake View', 'View of a lake'),
('Rooftop', 'Rooftop access or terrace'),
('Hot Tub', 'Hot tub or jacuzzi available');

-- Create indexes for better performance
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_UserType ON Users(UserType);
CREATE INDEX IX_Properties_OwnerId ON Properties(OwnerId);
CREATE INDEX IX_Properties_City ON Properties(City);
CREATE INDEX IX_Properties_PropertyTypeId ON Properties(PropertyTypeId);
CREATE INDEX IX_Properties_IsActive ON Properties(IsActive);
CREATE INDEX IX_Properties_IsAvailable ON Properties(IsAvailable);
CREATE INDEX IX_PropertyImages_PropertyId ON PropertyImages(PropertyId);
CREATE INDEX IX_Reservations_PropertyId ON Reservations(PropertyId);
CREATE INDEX IX_Reservations_RenterId ON Reservations(RenterId);
CREATE INDEX IX_Reservations_Status ON Reservations(Status);
CREATE INDEX IX_Messages_PropertyId ON Messages(PropertyId);
CREATE INDEX IX_Messages_SenderId ON Messages(SenderId);
CREATE INDEX IX_Messages_ReceiverId ON Messages(ReceiverId);
CREATE INDEX IX_Reviews_PropertyId ON Reviews(PropertyId);
CREATE INDEX IX_Reviews_RenterId ON Reviews(RenterId);
CREATE INDEX IX_Notifications_UserId ON Notifications(UserId);
CREATE INDEX IX_Notifications_IsRead ON Notifications(IsRead);

-- Create a view for property search
CREATE VIEW PropertySearchView AS
SELECT 
    p.Id,
    p.Title,
    p.Description,
    p.Address,
    p.City,
    p.State,
    p.Country,
    p.PricePerNight,
    p.MaxGuests,
    p.Bedrooms,
    p.Bathrooms,
    p.SquareFootage,
    pt.Name AS PropertyType,
    u.FirstName + ' ' + u.LastName AS OwnerName,
    u.Email AS OwnerEmail,
    u.PhoneNumber AS OwnerPhone,
    (SELECT AVG(CAST(r.Rating AS FLOAT)) FROM Reviews r WHERE r.PropertyId = p.Id) AS AverageRating,
    (SELECT COUNT(*) FROM Reviews r WHERE r.PropertyId = p.Id) AS ReviewCount,
    (SELECT TOP 1 pi.ImagePath FROM PropertyImages pi WHERE pi.PropertyId = p.Id AND pi.IsPrimary = 1) AS PrimaryImagePath
FROM Properties p
INNER JOIN PropertyTypes pt ON p.PropertyTypeId = pt.Id
INNER JOIN Users u ON p.OwnerId = u.Id
WHERE p.IsActive = 1 AND p.IsAvailable = 1;

GO
