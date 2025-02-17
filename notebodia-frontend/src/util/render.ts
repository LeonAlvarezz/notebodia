import { NIcon } from 'naive-ui'
import { h, type Component } from 'vue'

export function renderIcon(icon: Component, className?: string) {
    return () => {
        return h(
            NIcon,
            { class: className },
            {
                default: () => h(icon),
            }
        )
    }
}
