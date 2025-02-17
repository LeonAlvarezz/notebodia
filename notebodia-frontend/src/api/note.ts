import type { CreateNewNote, Note, NoteFilter } from '@/types/note.type'
import { fetchApi } from '@/util/fetch-client'
import { toQueryString } from '@/util/string'

const prefix = '/notes'
export async function getUserNote(filters: NoteFilter) {
    const fullUrl = `${prefix}${toQueryString({ ...filters })}`

    const res = await fetchApi.get<Note[]>(fullUrl)
    return res
}

export async function createNote(payload: CreateNewNote) {
    const res = await fetchApi.post<Note>(prefix, payload)
    return res
}

export async function updateNote(id: string, payload: CreateNewNote) {
    const res = await fetchApi.put<Note>(`${prefix}/${id}`, payload)
    return res
}

export async function deleteNote(id: string) {
    const res = await fetchApi.delete(`${prefix}/${id}`)
    return res
}
