export interface Category {
  id: string;
  name: string;
  icon: string | null;
  parentCategoryId: string | null;
  isRoot: boolean;
  children: Category[];
  createdAt: string;
  updatedAt: string;
}

export interface CreateCategoryRequest {
  name: string;
  icon: string | null;
  parentCategoryId: string | null;
}