import './style.css'

/**
 * 应用初始化完成之后再进行页面加载渲染
 */
async function initApplication() {
    // const env = import.meta.env.PROD ? 'prod' : 'dev'
    // const appVersion = import.meta.env.VITE_APP_VERSION
    // const namespace = `${import.meta.env.VITE_APP_NAMESPACE}-${appVersion}-${env}`

    const { bootstrap } = await import('./bootstrap')
    await bootstrap()

    // 移除并销毁loading
}

initApplication()
