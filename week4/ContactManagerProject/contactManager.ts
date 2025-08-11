// Contact interface
interface Contact {
    id: number;
    name: string;
    email: string;
    phone: string;
}

// Contact Manager class
class ContactManager {
    contacts: Contact[] = [];

    // Add contact
    addContact(contact: Contact) {
        this.contacts.push(contact);
        console.log(`Contact added: ${contact.name}`);
    }

    // View all contacts
    viewContacts() {
        if (this.contacts.length === 0) {
            console.log("No contacts found");
        } else {
            console.log("Contact List:");
            this.contacts.forEach(c => {
                console.log(`ID: ${c.id}, Name: ${c.name}, Email: ${c.email}, Phone: ${c.phone}`);
            });
        }
    }

    // Modify contact
    modifyContact(id: number, updated: Partial<Contact>) {
        let found = this.contacts.find(c => c.id === id);
        if (!found) {
            console.log("Contact not found");
            return;
        }
        Object.assign(found, updated);
        console.log(`Contact with ID ${id} updated`);
    }

    // Delete contact
    deleteContact(id: number) {
        let index = this.contacts.findIndex(c => c.id === id);
        if (index === -1) {
            console.log("Contact not found");
            return;
        }
        let deleted = this.contacts.splice(index, 1);
        console.log(`Deleted contact: ${deleted[0].name}`);
    }
}

// Testing
let manager = new ContactManager();

manager.addContact({ id: 1, name: "Khushi", email: "khushi@mail.com", phone: "9876543210" });
manager.addContact({ id: 2, name: "Aniket", email: "aniket@mail.com", phone: "9876501234" });

manager.viewContacts();

manager.modifyContact(1, { phone: "9998887770" });

manager.viewContacts();

manager.deleteContact(2);

manager.viewContacts();
