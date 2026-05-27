export interface Page {
  component: string;
  props: Record<string, any>;
  url: string;
  version: string | null;
}

export interface PageProps {
  [key: string]: any;
}

export interface InertiaAppProps {
  initialPage: Page;
  resolveComponent: (name: string) => Promise<React.ComponentType<PageProps>> | React.ComponentType<PageProps>;
}

export interface SharedData {
  [key: string]: any;
}

export interface InertiaLinkProps {
  href: string;
  method?: 'get' | 'post' | 'put' | 'patch' | 'delete';
  data?: Record<string, any>;
  headers?: Record<string, string>;
  as?: 'a' | 'button';
  replace?: boolean;
  preserveState?: boolean | ((props: PageProps) => boolean);
  preserveScroll?: boolean | ((props: PageProps) => boolean);
  only?: string[];
  except?: string[];
  onClick?: (event: React.MouseEvent) => void;
  onCancelToken?: (cancelToken: any) => void;
  onBefore?: (visit: any) => void;
  onStart?: (visit: any) => void;
  onProgress?: (progress: any) => void;
  onSuccess?: (page: Page) => void;
  onError?: (errors: any) => void;
  onCancel?: () => void;
  onFinish?: (visit: any) => void;
}
