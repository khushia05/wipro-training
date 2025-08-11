var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
// Step 2: Implement the ContactManager class
var ContactManager = /** @class */ (function () {
    function ContactManager() {
        this.contacts = [];
    }
    // Add a new contact
    ContactManager.prototype.addContact = function (contact) {
        // Check if ID already exists
        if (this.getContactIndex(contact.id) !== -1) {
            console.log("Error: Contact with ID ".concat(contact.id, " already exists."));
            return;
        }
        this.contacts.push(contact);
        console.log("Contact with ID ".concat(contact.id, " added successfully."));
    };
    // View all contacts
    ContactManager.prototype.viewContacts = function () {
        if (this.contacts.length === 0) {
            console.log("No contacts found.");
        }
        return this.contacts;
    };
    // Modify an existing contact
    ContactManager.prototype.modifyContact = function (id, updatedContact) {
        var index = this.getContactIndex(id);
        if (index === -1) {
            console.log("Error: Contact with ID ".concat(id, " not found."));
            return;
        }
        this.contacts[index] = __assign(__assign({}, this.contacts[index]), updatedContact);
        console.log("Contact with ID ".concat(id, " modified successfully."));
    };
    // Delete a contact
    ContactManager.prototype.deleteContact = function (id) {
        var index = this.getContactIndex(id);
        if (index === -1) {
            console.log("Error: Contact with ID ".concat(id, " not found."));
            return;
        }
        this.contacts.splice(index, 1);
        console.log("Contact with ID ".concat(id, " deleted successfully."));
    };
    // Helper method to find contact index without using findIndex()
    ContactManager.prototype.getContactIndex = function (id) {
        for (var i = 0; i < this.contacts.length; i++) {
            if (this.contacts[i].id === id) {
                return i;
            }
        }
        return -1;
    };
    return ContactManager;
}());
// Step 4: Testing
var manager = new ContactManager();
manager.addContact({ id: 1, name: "John Doe", email: "john@example.com", phone: "12345" });
manager.addContact({ id: 2, name: "Alice Smith", email: "alice@example.com", phone: "67890" });
console.log("Contacts:", manager.viewContacts());
manager.modifyContact(1, { email: "john.doe@example.com" });
console.log("Contacts after modification:", manager.viewContacts());
manager.deleteContact(2);
console.log("Contacts after deletion:", manager.viewContacts());
manager.deleteContact(3); // Testing error case
