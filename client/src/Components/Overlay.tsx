import classes from './Overlay.module.scss';

export enum OverlaySize {
    FULLSCREEN,
    THIN
}

type OverlayProps = {
    children: React.ReactNode,
    classname?: string,
    size?: OverlaySize,
    onOutsideClick: () => void
}

const Overlay = ({ children, classname = "", size = OverlaySize.FULLSCREEN, onOutsideClick }: OverlayProps) => {

    const getSizeClassName = () => {
        switch (size) {
            case OverlaySize.FULLSCREEN:
                return classes.overlayFullscreen;
            case OverlaySize.THIN:
                return classes.overlayThin;
            default:
                return "";
        }
    }

    return (
        <div className={`d-flex ${classes.overlayBackground} position-fixed w-100 h-100`} onClick={onOutsideClick}>
            <div className={`${classname} ${getSizeClassName()} rounded`} onClick={e => e.stopPropagation()}>
                {children}
            </div>
        </div >
    )
}

export default Overlay