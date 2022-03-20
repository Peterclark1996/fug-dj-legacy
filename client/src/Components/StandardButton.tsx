import classes from './StandardButton.module.scss'

type StandardButtonProps = {
    className?: string
    text?: string
    iconClasses?: string
    toolTipText?: string
    isDisabled?: boolean
    onClick: () => void
}

const StandardButton = ({ className = "", text, iconClasses, toolTipText, isDisabled = false, onClick }: StandardButtonProps) => {
    return (
        <div role='button' className={`${className} ${classes.toolTip} btn btn-warning ${isDisabled && "disabled"}`} onClick={onClick}>
            {iconClasses && <i className={iconClasses} />}
            {text}
            {toolTipText && <span className={`text-nowrap px-1 rounded bg-dark text-white ${classes.toolTipText}`}>{toolTipText}</span>}
        </div>
    )
}

export default StandardButton