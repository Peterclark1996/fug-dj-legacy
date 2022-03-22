import classes from './Overlay.module.scss';

type OverlayProps = {
    children: React.ReactNode,
    classname?: string,
    onOutsideClick: () => void
}

const Overlay = ({ children, classname = "", onOutsideClick }: OverlayProps) => {
    return (
        <div className={`${classes.overlayBackground} position-fixed w-100 h-100`} onClick={onOutsideClick}>
            <div className={`${classname} ${classes.overlay} rounded`} onClick={e => e.stopPropagation()}>
                {children}
            </div>
        </div >
    )
}

export default Overlay