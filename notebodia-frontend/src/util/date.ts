export function formatDate(date: string | Date) {
    const dateObj = new Date(date)
    return dateObj.toLocaleDateString('en-US', {
        day: 'numeric',
        month: 'long',
        year: 'numeric',
    })
}

export function formatDateTime(date: string | Date) {
    const dateObj = new Date(date)

    // Option 1: "Mar 3, 5:49 PM"
    return dateObj.toLocaleString('en-US', {
        month: 'short',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
    })
}
