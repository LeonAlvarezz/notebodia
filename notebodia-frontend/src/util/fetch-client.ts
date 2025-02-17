import type { HttpError } from '@/types/base.type'

export async function apiFetch<T>(
    url: string,
    options: RequestInit = {}
): Promise<{ data: T | null; error: HttpError | null; success: boolean }> {
    const baseUrl = import.meta.env.VITE_API_URL || 'http://localhost:3000'

    const defaultOptions: RequestInit = {
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',

            ...options.headers,
        },
    }
    const mergedOptions = {
        ...defaultOptions,
        ...options,
        headers: {
            ...defaultOptions.headers,
            ...options.headers,
        },
    }

    try {
        const response = await fetch(`${baseUrl}${url}`, mergedOptions)
        console.log('YOU ARE HITTING THIS ENDPOINT 👉:', url)

        const contentType = response.headers.get('content-type')

        if (!response.ok) {
            const errorBody = await response.json()
            return {
                data: null,
                error: {
                    statusCode: errorBody.statusCode || 500,
                    message: errorBody.message,
                },
                success: false,
            }
        }

        if (contentType && contentType.includes('application/json')) {
            const json = await response.json()
            return {
                data: json.data as T,
                error: null,
                success: true,
            }
        }

        return {
            data: (await response.text()) as T,
            error: null,
            success: true,
        }
    } catch (error) {
        return {
            data: null,
            error: {
                statusCode: 500,
                message:
                    error instanceof Error
                        ? error.message
                        : 'Unknown error occurred',
            },
            success: false,
        }
    }
}

// HTTP method helpers
export const fetchApi = {
    get: <T>(url: string, options?: RequestInit) =>
        apiFetch<T>(url, { method: 'GET', ...options }),
    post: <T>(url: string, body?: unknown, options?: RequestInit) =>
        apiFetch<T>(url, {
            method: 'POST',
            body: body ? JSON.stringify(body) : undefined,
            ...options,
        }),
    put: <T>(url: string, body?: unknown, options?: RequestInit) =>
        apiFetch<T>(url, {
            method: 'PUT',
            body: body ? JSON.stringify(body) : undefined,
            ...options,
        }),
    delete: <T>(url: string, options?: RequestInit) =>
        apiFetch<T>(url, { method: 'DELETE', ...options }),
}
