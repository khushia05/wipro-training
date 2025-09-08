# RentAPlace - Use Case Document

## Project Overview
RentAPlace is an online platform for renting homes for short and long-term durations. The application serves two types of users: regular users (renters) and property owners.

## Actors
1. **Renter** - A user who searches for and books properties
2. **Owner** - A user who manages property listings
3. **System** - The application itself

## Use Cases

### Renter Use Cases

#### UC-001: User Registration
- **Actor**: Renter
- **Description**: A new user registers for an account
- **Preconditions**: User is not already registered
- **Main Flow**:
  1. User navigates to registration page
  2. User fills in personal details (name, email, password, phone)
  3. User selects account type (Renter)
  4. System validates input data
  5. System creates new user account
  6. System sends confirmation email
  7. User receives confirmation and can log in

#### UC-002: User Login
- **Actor**: Renter
- **Description**: Registered user logs into the system
- **Preconditions**: User has a valid account
- **Main Flow**:
  1. User navigates to login page
  2. User enters email and password
  3. System validates credentials
  4. System generates JWT token
  5. User is redirected to dashboard

#### UC-003: User Logout
- **Actor**: Renter
- **Description**: User logs out of the system
- **Main Flow**:
  1. User clicks logout button
  2. System invalidates JWT token
  3. User is redirected to home page

#### UC-004: View Top-Rated Properties
- **Actor**: Renter
- **Description**: User views properties sorted by rating
- **Main Flow**:
  1. User navigates to home page
  2. System displays top-rated properties by category
  3. User can filter by property type, location, or features

#### UC-005: Search Properties
- **Actor**: Renter
- **Description**: User searches for properties based on criteria
- **Main Flow**:
  1. User enters search criteria (dates, location, type, features)
  2. System searches database for matching properties
  3. System displays search results
  4. User can further filter results

#### UC-006: View Property Details
- **Actor**: Renter
- **Description**: User views detailed information about a property
- **Main Flow**:
  1. User clicks on a property from search results
  2. System displays property details (4-5 images, description, amenities)
  3. User can view availability calendar
  4. User can see owner information

#### UC-007: Reserve Property
- **Actor**: Renter
- **Description**: User makes a reservation for a property
- **Preconditions**: User is logged in
- **Main Flow**:
  1. User selects property and dates
  2. User clicks "Reserve" button
  3. System checks availability
  4. User confirms reservation details
  5. System creates reservation
  6. System sends confirmation to user and owner

#### UC-008: Send Message to Owner
- **Actor**: Renter
- **Description**: User sends a message to property owner
- **Preconditions**: User is logged in
- **Main Flow**:
  1. User navigates to property details
  2. User clicks "Contact Owner" button
  3. User types message
  4. System sends message to owner
  5. Owner receives notification

### Owner Use Cases

#### UC-009: Owner Registration
- **Actor**: Owner
- **Description**: Property owner registers for an account
- **Preconditions**: Owner is not already registered
- **Main Flow**:
  1. Owner navigates to registration page
  2. Owner fills in personal details
  3. Owner selects account type (Owner)
  4. System validates input data
  5. System creates new owner account
  6. System sends confirmation email

#### UC-010: Owner Login
- **Actor**: Owner
- **Description**: Property owner logs into the system
- **Preconditions**: Owner has a valid account
- **Main Flow**:
  1. Owner navigates to login page
  2. Owner enters email and password
  3. System validates credentials
  4. System generates JWT token
  5. Owner is redirected to dashboard

#### UC-011: Add Property
- **Actor**: Owner
- **Description**: Owner adds a new property listing
- **Preconditions**: Owner is logged in
- **Main Flow**:
  1. Owner navigates to "Add Property" page
  2. Owner fills in property details (title, description, type, location)
  3. Owner uploads 4-5 property images
  4. Owner sets pricing and availability
  5. Owner selects property features
  6. System validates property data
  7. System creates property listing

#### UC-012: Update Property
- **Actor**: Owner
- **Description**: Owner updates existing property information
- **Preconditions**: Owner is logged in and owns the property
- **Main Flow**:
  1. Owner navigates to property management page
  2. Owner selects property to update
  3. Owner modifies property details
  4. Owner uploads new images if needed
  5. System validates updated data
  6. System updates property listing

#### UC-013: Delete Property
- **Actor**: Owner
- **Description**: Owner removes property from platform
- **Preconditions**: Owner is logged in and owns the property
- **Main Flow**:
  1. Owner navigates to property management page
  2. Owner selects property to delete
  3. Owner confirms deletion
  4. System removes property and associated data

#### UC-014: View Messages
- **Actor**: Owner
- **Description**: Owner views messages from renters
- **Preconditions**: Owner is logged in
- **Main Flow**:
  1. Owner navigates to messages page
  2. System displays all messages from renters
  3. Owner can filter messages by property or date

#### UC-015: Reply to Messages
- **Actor**: Owner
- **Description**: Owner responds to renter messages
- **Preconditions**: Owner is logged in
- **Main Flow**:
  1. Owner selects a message to reply to
  2. Owner types response
  3. System sends reply to renter
  4. Renter receives notification

#### UC-016: Confirm Reservation
- **Actor**: Owner
- **Description**: Owner confirms or rejects a reservation
- **Preconditions**: Owner is logged in and has pending reservations
- **Main Flow**:
  1. Owner receives email notification of new reservation
  2. Owner navigates to reservations page
  3. Owner reviews reservation details
  4. Owner confirms or rejects reservation
  5. System updates reservation status
  6. Renter receives notification of decision

### System Use Cases

#### UC-017: Send Email Notifications
- **Actor**: System
- **Description**: System sends email notifications for various events
- **Main Flow**:
  1. System detects notification event (reservation, message, etc.)
  2. System generates appropriate email template
  3. System sends email to relevant user
  4. System logs notification status

#### UC-018: Manage File Uploads
- **Actor**: System
- **Description**: System handles property image uploads
- **Main Flow**:
  1. User uploads image file
  2. System validates file type and size
  3. System stores file in designated folder
  4. System updates database with file path
  5. System returns success confirmation

## Non-Functional Requirements
- Response time: API calls should respond within 2 seconds
- Availability: System should be available 99.9% of the time
- Security: All user data must be encrypted and secure
- Scalability: System should handle up to 10,000 concurrent users
- File storage: Property images should be stored securely with backup

## Technical Requirements
- Backend: ASP.NET Core Web API with Entity Framework Core
- Frontend: Angular with TypeScript
- Database: SQL Server
- Authentication: JWT Bearer tokens
- File Storage: Local file system with designated folder
- API Documentation: Swagger/OpenAPI
