import {ref} from 'vue';

export function useApiBaseUrl() {
    const apiBaseUrl = ref(import.meta.env.VITE_API_BASE_URL || 'http://localhost:5039/api');
    return { apiBaseUrl };
}
