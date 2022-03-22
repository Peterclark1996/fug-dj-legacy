type LoadingProps = {
    isLoading: boolean
    children: React.ReactNode
}

const Loading = ({ isLoading, children }: LoadingProps) => {
    if (isLoading) {
        return (
            <div className="w-100 h-100">
                <div className="d-flex justify-content-center">
                    <div className="spinner-border text-warning" />
                </div>
                <div style={{ visibility: 'hidden' }}>{children}</div>
            </div>
        )
    }
    return (
        <div>
            {children}
        </div>
    )
}

export default Loading