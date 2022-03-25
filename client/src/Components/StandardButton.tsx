import classes from './StandardButton.module.scss'

type StandardButtonProps = {
    className?: string
    text?: string
    textClasses?: string
    iconClasses?: string
    toolTipText?: string
    isFixedSize?: boolean
    isDisabled?: boolean
    onClick: () => void
}

const StandardButton = ({ className = "", text, textClasses = "", iconClasses = "", toolTipText, isFixedSize = false, isDisabled = false, onClick }: StandardButtonProps) => {
    return (
        <div role="button" className={`${className} ${classes.toolTip} d-flex justify-content-center align-items-center btn ${classes.buttonColour} ${isFixedSize && classes.fixedSize} ${isDisabled && "disabled"}`} onClick={onClick}>
            {iconClasses && <i className={iconClasses} />}
            <span className={textClasses}>{text}</span>
            {toolTipText && <span className={`text-nowrap px-1 rounded bg-dark text-white ${classes.toolTipText}`}>{toolTipText}</span>}
        </div>
    )
}

export default StandardButton