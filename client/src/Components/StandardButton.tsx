import classes from './StandardButton.module.scss'

export enum ButtonSize {
    DYNAMIC,
    SMALL,
    LARGE
}

export enum ToolTipDirection {
    LEFT,
    RIGHT
}

type StandardButtonProps = {
    className?: string
    text?: string
    textClasses?: string
    iconClasses?: string
    toolTipText?: string
    toolTipDirection?: ToolTipDirection
    size?: ButtonSize
    isDisabled?: boolean
    onClick: () => void
}

const StandardButton = ({ className = "", text, textClasses = "", iconClasses = "", toolTipText, toolTipDirection = ToolTipDirection.RIGHT, size = ButtonSize.DYNAMIC, isDisabled = false, onClick }: StandardButtonProps) => {
    const toolTipDirectionClass = toolTipDirection === ToolTipDirection.LEFT ? classes.toolTipLeft : classes.toolTipRight

    return (
        <div
            role="button"
            className={`${className} ${classes.toolTip} ${toolTipDirectionClass} d-flex justify-content-center align-items-center btn ${classes.buttonColour} ${size === ButtonSize.SMALL && classes.fixedSizeSmall} ${size === ButtonSize.LARGE && classes.fixedSizeLarge} ${isDisabled && "disabled"}`}
            onClick={onClick}
        >
            {iconClasses && <i className={iconClasses} />}
            <span className={textClasses}>{text}</span>
            {toolTipText && <span className={`text-nowrap px-1 rounded bg-dark text-white ${classes.toolTipText} ${toolTipDirectionClass}`}>{toolTipText}</span>}
        </div>
    )
}

export default StandardButton