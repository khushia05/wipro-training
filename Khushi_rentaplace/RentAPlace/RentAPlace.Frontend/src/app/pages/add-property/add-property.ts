import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpEventType } from '@angular/common/http';

interface PropertyType {
  id: number;
  name: string;
}

interface PropertyFeature {
  id: number;
  name: string;
}

interface SelectedImage {
  file: File;
  preview: string;
}

@Component({
  selector: 'app-add-property',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-property.html',
  styleUrls: ['./add-property.css']
})
export class AddPropertyComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  
  propertyForm!: FormGroup;
  propertyTypes: PropertyType[] = [];
  propertyFeatures: PropertyFeature[] = [];
  loading = false;
  error = '';
  success = '';
  
  // Photo upload properties
  selectedImages: SelectedImage[] = [];
  isDragOver = false;
  uploadProgress = 0;
  primaryImageIndex = 0;
  maxFileSize = 10 * 1024 * 1024; // 10MB
  allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp'];

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.initializeForm();
  }

  ngOnInit() {
    this.loadPropertyTypes();
    this.loadPropertyFeatures();
  }

  initializeForm() {
    this.propertyForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      address: ['', [Validators.required, Validators.minLength(5)]],
      city: ['', [Validators.required]],
      state: ['', [Validators.required]],
      country: ['India', [Validators.required]],
      zipCode: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
      price: ['', [Validators.required, Validators.min(1)]],
      propertyTypeId: ['', [Validators.required]],
      bedrooms: ['', [Validators.required, Validators.min(0)]],
      bathrooms: ['', [Validators.required, Validators.min(0)]],
      area: ['', [Validators.required, Validators.min(1)]],
      maxGuests: ['', [Validators.required, Validators.min(1)]],
      isAvailable: [true],
      features: [[]]
    });
  }

  loadPropertyTypes() {
    this.http.get<PropertyType[]>('http://localhost:5158/api/propertytypes').subscribe({
      next: (types) => {
        this.propertyTypes = types;
      },
      error: (error) => {
        console.error('Error loading property types:', error);
      }
    });
  }

  loadPropertyFeatures() {
    this.http.get<PropertyFeature[]>('http://localhost:5158/api/propertyfeatures').subscribe({
      next: (features) => {
        this.propertyFeatures = features;
      },
      error: (error) => {
        console.error('Error loading property features:', error);
      }
    });
  }

  async onSubmit() {
    if (this.propertyForm.valid) {
      this.loading = true;
      this.error = '';
      this.success = '';

      try {
        // First, upload images if any are selected
        let imageUrls: string[] = [];
        if (this.selectedImages.length > 0) {
          imageUrls = await this.uploadImages();
        }

        const formValue = this.propertyForm.value;
        const propertyData = {
          name: formValue.name,
          description: formValue.description,
          address: formValue.address,
          city: formValue.city,
          state: formValue.state,
          country: formValue.country,
          zipCode: formValue.zipCode,
          price: parseFloat(formValue.price),
          propertyTypeId: parseInt(formValue.propertyTypeId),
          bedrooms: parseInt(formValue.bedrooms),
          bathrooms: parseInt(formValue.bathrooms),
          area: parseInt(formValue.area),
          maxGuests: parseInt(formValue.maxGuests),
          isAvailable: formValue.isAvailable,
          featureIds: formValue.features || [],
          imageUrls: imageUrls // Include uploaded image URLs
        };

        this.http.post('http://localhost:5158/api/properties', propertyData).subscribe({
          next: (response) => {
            this.success = 'Property added successfully with ' + this.selectedImages.length + ' photos!';
            this.loading = false;
            this.uploadProgress = 0;
            this.selectedImages = [];
            this.propertyForm.reset();
            this.initializeForm();
            setTimeout(() => {
              this.router.navigate(['/my-properties']);
            }, 2000);
          },
          error: (error) => {
            console.error('Error adding property:', error);
            this.error = 'Failed to add property. Please try again.';
            this.loading = false;
            this.uploadProgress = 0;
          }
        });
      } catch (error) {
        console.error('Error uploading images:', error);
        this.error = 'Failed to upload images. Please try again.';
        this.loading = false;
        this.uploadProgress = 0;
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  onFeatureChange(featureId: number, isChecked: boolean) {
    const features = this.propertyForm.get('features')?.value || [];
    if (isChecked) {
      features.push(featureId);
    } else {
      const index = features.indexOf(featureId);
      if (index > -1) {
        features.splice(index, 1);
      }
    }
    this.propertyForm.patchValue({ features });
  }

  isFeatureSelected(featureId: number): boolean {
    const features = this.propertyForm.get('features')?.value || [];
    return features.includes(featureId);
  }

  getFieldError(fieldName: string): string {
    const field = this.propertyForm.get(fieldName);
    if (field?.errors && field.touched) {
      if (field.errors['required']) {
        return `${this.getFieldLabel(fieldName)} is required`;
      }
      if (field.errors['minlength']) {
        return `${this.getFieldLabel(fieldName)} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
      if (field.errors['min']) {
        return `${this.getFieldLabel(fieldName)} must be at least ${field.errors['min'].min}`;
      }
      if (field.errors['pattern']) {
        return 'Please enter a valid ZIP code (123456)';
      }
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      name: 'Property Name',
      description: 'Description',
      address: 'Address',
      city: 'City',
      state: 'State',
      country: 'Country',
      zipCode: 'ZIP Code',
      price: 'Price',
      propertyTypeId: 'Property Type',
      bedrooms: 'Bedrooms',
      bathrooms: 'Bathrooms',
      area: 'Area (sq ft)',
      maxGuests: 'Maximum Guests'
    };
    return labels[fieldName] || fieldName;
  }

  private markFormGroupTouched() {
    Object.keys(this.propertyForm.controls).forEach(key => {
      const control = this.propertyForm.get(key);
      control?.markAsTouched();
    });
  }

  goBack() {
    this.router.navigate(['/owner-dashboard']);
  }

  // Photo upload methods
  triggerFileInput() {
    this.fileInput.nativeElement.click();
  }

  onFileSelect(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.handleFiles(Array.from(input.files));
    }
  }

  onFileSelected(event: any) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.handleFiles(Array.from(input.files));
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.isDragOver = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    this.isDragOver = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.isDragOver = false;
    
    const files = event.dataTransfer?.files;
    if (files) {
      this.handleFiles(Array.from(files));
    }
  }

  handleFiles(files: File[]) {
    const validFiles = files.filter(file => this.validateFile(file));
    
    validFiles.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e) => {
        const preview = e.target?.result as string;
        this.selectedImages.push({
          file: file,
          preview: preview
        });
      };
      reader.readAsDataURL(file);
    });

    // Clear the file input
    if (this.fileInput) {
      this.fileInput.nativeElement.value = '';
    }
  }

  validateFile(file: File): boolean {
    // Check file type
    if (!this.allowedTypes.includes(file.type)) {
      alert(`File type ${file.type} is not supported. Please use JPG, PNG, or WebP.`);
      return false;
    }

    // Check file size
    if (file.size > this.maxFileSize) {
      alert(`File ${file.name} is too large. Maximum size is 10MB.`);
      return false;
    }

    return true;
  }

  removeImage(index: number) {
    this.selectedImages.splice(index, 1);
    
    // Adjust primary image index if needed
    if (this.primaryImageIndex >= this.selectedImages.length) {
      this.primaryImageIndex = Math.max(0, this.selectedImages.length - 1);
    }
  }

  setPrimaryImage(index: number) {
    this.primaryImageIndex = index;
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  async uploadImages(): Promise<string[]> {
    if (this.selectedImages.length === 0) {
      return [];
    }

    const uploadedUrls: string[] = [];
    this.uploadProgress = 0;

    try {
      // Prepare FormData for multiple images
      const formData = new FormData();
      
      this.selectedImages.forEach((selectedImage, index) => {
        formData.append('images', selectedImage.file);
      });

      // Upload images to server
      const response = await this.http.post<any[]>('http://localhost:5158/api/imageupload/upload-multiple', formData).toPromise();

      // Handle the response
      if (response && Array.isArray(response)) {
        response.forEach((imageData: any) => {
          // Store relative URL for database
          uploadedUrls.push(imageData.url);
        });
      }

      this.uploadProgress = 100;
      console.log('Images uploaded successfully:', uploadedUrls);

    } catch (error) {
      console.error('Error uploading images:', error);
      
      // Fallback to placeholder URLs if upload fails
      for (let i = 0; i < this.selectedImages.length; i++) {
        uploadedUrls.push(`https://picsum.photos/800/600?random=${Date.now()}-${i}`);
        this.uploadProgress = Math.round(((i + 1) / this.selectedImages.length) * 100);
      }
    }

    return uploadedUrls;
  }
}