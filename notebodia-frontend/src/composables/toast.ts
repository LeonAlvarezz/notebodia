import { createDiscreteApi, darkTheme, type MessageOptions } from 'naive-ui'
import type { VNodeChild } from 'vue'

type ContentType = string | (() => VNodeChild)

export function toast(content: ContentType, props?: MessageOptions) {
    const { message } = createDiscreteApi(['message'], {
        configProviderProps: {
            theme: darkTheme,
        },
    })

    const toast = message.create(content, {
        duration: props?.duration || 3000,
        ...props,
    })

    return toast
}
