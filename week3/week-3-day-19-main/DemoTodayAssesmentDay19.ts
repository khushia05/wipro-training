// Step 1: Define the Contact interface
interface Contact {
  id: number;
  name: string;
  email: string;
  phone: string;
}

// Step 2: Implement the ContactManager class
class ContactManager {
  private contacts: Contact[] = [];

  // Add a new contact
  addContact(contact: Contact): void {
    // Check if ID already exists
    if (this.getContactIndex(contact.id) !== -1) {
      console.log(`Error: Contact with ID ${contact.id} already exists.`);
      return;
    }
    this.contacts.push(contact);
    console.log(`Contact with ID ${contact.id} added successfully.`);
  }

  // View all contacts
  viewContacts(): Contact[] {
    if (this.contacts.length === 0) {
      console.log("No contacts found.");
    }
    return this.contacts;
  }

  // Modify an existing contact
  modifyContact(id: number, updatedContact: Partial<Contact>): void {
    const index = this.getContactIndex(id);
    if (index === -1) {
      console.log(`Error: Contact with ID ${id} not found.`);
      return;
    }
    this.contacts[index] = { ...this.contacts[index], ...updatedContact };
    console.log(`Contact with ID ${id} modified successfully.`);
  }

  // Delete a contact
  deleteContact(id: number): void {
    const index = this.getContactIndex(id);
    if (index === -1) {
      console.log(`Error: Contact with ID ${id} not found.`);
      return;
    }
    this.contacts.splice(index, 1);
    console.log(`Contact with ID ${id} deleted successfully.`);
  }

  // Helper method to find contact index without using findIndex()
  private getContactIndex(id: number): number {
    for (let i = 0; i < this.contacts.length; i++) {
      if (this.contacts[i].id === id) {
        return i;
      }
    }
    return -1;
  }
}

// Step 4: Testing
const manager = new ContactManager();

manager.addContact({ id: 1, name: "John Doe", email: "john@example.com", phone: "12345" });
manager.addContact({ id: 2, name: "Alice Smith", email: "alice@example.com", phone: "67890" });

console.log("Contacts:", manager.viewContacts());

manager.modifyContact(1, { email: "john.doe@example.com" });
console.log("Contacts after modification:", manager.viewContacts());

manager.deleteContact(2);
console.log("Contacts after deletion:", manager.viewContacts());

manager.deleteContact(3); // Testing error case
